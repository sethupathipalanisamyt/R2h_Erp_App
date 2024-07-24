using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using R2h_Erp_App.DbModels;
using R2h_Erp_App.Models;
using System.Security.Cryptography;




namespace R2h_Erp_App.Controllers
{
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext _context;

        public OrderTabController(R2hErpDbContext context)
        {
            _context = context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            var show = _context.OrderTabs.Include(o => o.Customer).Include(p => p.Status).Where(o => !o.IsDeleted==true).ToList();
            return View("List", show);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
        //    var response = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
        //    var find = _context.StatusTabs.ToList();

        //    ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
        //    ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
        //    ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
        //    return View("Create");
            

        //}

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.OrderTabs.Include(o => o.Customer).Include(o => o.Status).Where(o => !o.IsDeleted== true).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderItems = await _context.OrderItemTabs.Where(oi => oi.OrderId == id && !oi.IsDeleted).Include(oi => oi.Product).ToListAsync();

            var orderItemsDetails = orderItems.Select(oi => new
            {
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                TotalAmount = oi.TotalAmount,
                UnitPrice = oi.UnitPrice
            }).ToList();

            var viewModel = new
            {
                order.OrderNumber,
                CustomerName = order.Customer.Name,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                ShippingFee = order.ShippingFee,
                NetAmount = order.NetAmount,
                StatusName = order.Status.StatusName,
                OrderItems = orderItemsDetails
            };

            return Json(viewModel);
        }
        private List<OrdertabVM> GetOrderItemsFromSession()
        {
            var itemsJson = HttpContext.Session.GetString("order");
            return itemsJson != null ? JsonConvert.DeserializeObject<List<OrdertabVM>>(itemsJson) : new List<OrdertabVM>();
        }

        private void SaveOrderItemsToSession(List<OrdertabVM> items)
        {
            HttpContext.Session.SetString("order", JsonConvert.SerializeObject(items));
        }

        // GET: OrderTabController/Create
        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            var result = _context.Customers.Where(p => !p.Isdeleted && p.IsActive).ToList();
            var response = _context.Products.Where(p => !p.Isdeleted && p.IsActive).ToList();
            var find = _context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");


            if (id == 0)
            {
                var date = new OrdertabVM
                {
                    OrderDate = DateTime.Now
                };
                return View(date);
            }

            var order = await _context.OrderTabs.Include(o => o.Customer).Include(o => o.Status).Include(o => o.OrderItemTabs).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderItems = order.OrderItemTabs.Where(oi => !oi.IsDeleted).Select(oi => new OrdertabVM { ProductId = oi.ProductId, ProductName = oi.Product.Name, Quantity = oi.Quantity, UnitPrice = oi.UnitPrice, TotalAmount = oi.TotalAmount }).ToList();

            SaveOrderItemsToSession(orderItems);

            var viewModel = new OrdertabVM
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                ShippingFee = order.ShippingFee,
                NetAmount = order.NetAmount,
                StatusId = order.StatusId
            };

            ViewBag.OrderItems = JsonConvert.SerializeObject(orderItems);
            ViewBag.CustomersId = new SelectList(_context.Customers.Where(c => !c.Isdeleted && c.IsActive).ToList(), "CustomersId", "Name", order.CustomerId);
            ViewBag.ProductId = new SelectList(_context.Products.Where(p => !p.Isdeleted && p.IsActive).ToList(), "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(_context.StatusTabs.ToList(), "StatusId", "StatusName", order.StatusId);

            return View(viewModel);
        }

        // POST: OrderTabController/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrdertabVM ordervm)
        {
            var customers = _context.Customers.Where(p => !p.Isdeleted && p.IsActive).ToList();
            var products = _context.Products.Where(p => !p.Isdeleted && p.IsActive).ToList();
            var statuses = _context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(customers, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(products, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");

            if (ordervm.OrderId == 0)
            {
                OrderTab order = new OrderTab();
                order.OrderNumber = ordervm.OrderNumber;
                order.CustomerId = ordervm.CustomerId;
                order.OrderDate = ordervm.OrderDate;
                order.SubTotal = ordervm.SubTotal;
                order.Discount = ordervm.Discount;
                order.ShippingFee = ordervm.ShippingFee;
                order.NetAmount = ordervm.NetAmount;
                order.StatusId = ordervm.StatusId;

                _context.OrderTabs.Add(order);
                await _context.SaveChangesAsync();

                var orderItem = JsonConvert.DeserializeObject<List<OrderItemTab>>(HttpContext.Session.GetString("order"));
                if (orderItem != null)
                {
                    foreach (var item in orderItem)
                    {
                        item.OrderId = order.OrderId;
                        _context.OrderItemTabs.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.Remove("order");
                return RedirectToAction(nameof(List));
            }
            else
            {
                var order = await _context.OrderTabs.Include(o => o.OrderItemTabs).FirstOrDefaultAsync(o => o.OrderId == ordervm.OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                order.OrderNumber = ordervm.OrderNumber;
                order.CustomerId = ordervm.CustomerId;
                order.OrderDate = ordervm.OrderDate;
                order.SubTotal = ordervm.SubTotal;
                order.Discount = ordervm.Discount;
                order.ShippingFee = ordervm.ShippingFee;
                order.NetAmount = ordervm.NetAmount;
                order.StatusId = ordervm.StatusId;

                _context.Update(order);

                var existingOrderItems = GetOrderItemsFromSession();
                foreach (var item in existingOrderItems)
                {
                    var orderItem = order.OrderItemTabs.FirstOrDefault(oi => oi.ProductId == item.ProductId);
                    if (orderItem != null)
                    {
                        orderItem.Quantity = item.Quantity;
                        orderItem.UnitPrice = item.UnitPrice;
                        orderItem.TotalAmount = item.TotalAmount;
                        _context.Update(orderItem);
                    }
                    else
                    {
                        orderItem = new OrderItemTab
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalAmount = item.TotalAmount
                        };
                        _context.OrderItemTabs.Add(orderItem);
                    }
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("order");
                return RedirectToAction(nameof(List));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnitPrice(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.UnitPrice);
        }

        [HttpPost]
        public JsonResult AddItem(OrdertabVM newItem)
        {
            var items = GetOrderItemsFromSession();
            var product = _context.Products.Find(newItem.ProductId);
            if (product != null)
            {
                newItem.ProductName = product.Name;
                items.Add(newItem);
                SaveOrderItemsToSession(items);
            }
            return Json(items);
        }





        // POST: OrderTabController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.OrderTabs
                .Include(o => o.OrderItemTabs)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }


            order.IsDeleted = true;
            foreach (var item in order.OrderItemTabs)
            {
                item.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return View(List);
        }

    }

}