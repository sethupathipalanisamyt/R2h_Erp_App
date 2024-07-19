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
        OrderItemTab OIT=new OrderItemTab();
        private readonly R2hErpDbContext _context;

        public OrderTabController(R2hErpDbContext context)
        {
            _context = context;
        }
        // GET: OrderTabController
        public async Task<IActionResult> Index()
        {
            var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var respose = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var find = _context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(respose, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View();

          

        }
        [HttpGet]
        public IActionResult List()
        {
            var show = _context.OrderTabs.Include(o => o.Customer).Include(p => p.Status).ToList();
            return View("List", show);
        }

        // GET: OrderTabController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderTabController/Create
        public ActionResult Create()
        {
            var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var respose = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var find = _context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(respose, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View();
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrdertabVM oredertabvm)
        {
            var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var respose = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var find = _context.StatusTabs.ToList();
            OrderTab order = new OrderTab();


            if (oredertabvm != null)
            {
                order.OrderNumber = oredertabvm.OrderNumber;
                order.CustomerId = oredertabvm.CustomerId;
                order.OrderDate = oredertabvm.OrderDate;
                order.SubTotal = oredertabvm.SubTotal;
                order.Discount = oredertabvm.Discount;
                order.ShippingFee = oredertabvm.ShippingFee;
                order.NetAmount = oredertabvm.NetAmount;
                order.StatusId = oredertabvm.StatusId;
                //OIT.OrderId = order.OrderId;
                //OIT.ProductId = oredertabvm.ProductId;
                //OIT.Quantity = oredertabvm.Quantity;
                //OIT.UnitPrice = oredertabvm.UnitPrice;
                //OIT.TotalAmount= OIT.Quantity * OIT.UnitPrice;
                _context.Add(order);
                //_context.Add(OIT);
                _context.SaveChanges();
                return RedirectToAction(nameof(List));

            }
            HttpContext.Session.SetString("OrderTab",JsonConvert.SerializeObject(order));
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(respose, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            
            return View(oredertabvm);
        }
        //public async Task<IActionResult> AddProduct(OrdertabVM vm)
        //{
        //    try 
        //    { 
        //        if (ModelState.IsValid)
        //        { 
        //            if (vm.OrderId == 1)
        //            { Product product = new Product();
        //                using (var httpClient = new HttpClient())
        //                { using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "Product/" + vm.ProductId))
        //                    { string apiResponse = await response.Content.ReadAsStringAsync();
        //                        product = JsonConvert.DeserializeObject<Product>(apiResponse); 
        //                    }
        //                }
        //                if (product == null || product.ProductId == 0) 
        //                {
        //                    ModelState.AddModelError("ProductName", "Please add valid Product!");
        //                    return Json(ModelState);
        //                }
        //                OrderedProduct op = new OrderedProduct();
        //                op.ProductId = product.ProductId; op.ProductCode = product.ProductCode;
        //                op.ProductDescription = product.ProductDescription;
        //                op.UoM = vm.UoM; op.UnitPrice = vm.UnitPrice; 
        //                op.Quantity = vm.Quantity; op.IsActive = true;
        //                op.TotalAmount = Math.Round((decimal)(vm.UnitPrice * op.Quantity), 2);
        //                List<OrderedProduct> orderlist = new List<OrderedProduct>();
        //                if (HttpContext.Session.GetString("POI") == null) orderlist.Add(op); 
        //                else
        //                {
        //                    orderlist = JsonConvert.DeserializeObject<List<OrderedProduct>>(HttpContext.Session.GetString("POI")); 
        //                    orderlist.Add(op); 
        //                }
        //                var serializedRecords = JsonConvert.SerializeObject(orderlist);
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                return Json(orderlist.Where(x => x.IsActive).ToList()); 
        //            } 
        //            else if (vm.Flag == 2)
        //            { 
        //                var orderlist = JsonConvert.DeserializeObject<List<OrderedProduct>>(HttpContext.Session.GetString("POI"));
        //                foreach (var oi in orderlist) { if (oi.ProductId == vm.ProductId) oi.IsActive = false;
        //                }
        //                var serializedRecords = JsonConvert.SerializeObject(orderlist); 
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                orderlist = orderlist.Where(x => x.IsActive).ToList();
        //                return Json(orderlist);
        //            }
        //            else
        //            { 
        //                List<OrderedProduct> list = new List<OrderedProduct>();
        //                using (var httpClient = new HttpClient())
        //                {
        //                    using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "PurchaseOrderItem/GetPurchaseOrderItems?poId=" + vm.PoId))
        //                    {
        //                        string apiResponse = await response.Content.ReadAsStringAsync(); 
        //                        list = JsonConvert.DeserializeObject<List<OrderedProduct>>(apiResponse);
        //                    }
        //                } 
        //                var serializedRecords = JsonConvert.SerializeObject(list);
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                return Json(list);
        //            } 
        //        } return Json(ModelState);
        //    }
        //    catch (Exception ex) 
        //    { 
        //        return Json("Error: " + ex.Message);
        //    }
        //}

        public JsonResult getProductByUnitPrice(int ProductId)           
        {
            var details = (_context.Products.Where(option => option.ProductsId == ProductId));
            return Json(details);
        }
       
        [HttpPost]
        [HttpPost]
        public JsonResult AddItem(OrdertabVM newItem)
        {
            var itemsJson = HttpContext.Session.GetString("order");
            var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrdertabVM>>(itemsJson) : new List<OrdertabVM>();
            var product = _context.Products.Find(newItem.ProductId);
            if (product != null)
            {
                newItem.ProductName = product.Name; // Set the ProductName
                items.Add(newItem);
                HttpContext.Session.SetString("order", JsonConvert.SerializeObject(items));
            }
            return Json(items);
        }
        [HttpGet]
        public ActionResult PopupContent(OrdertabVM ordertab)
        {
            return View("Details", ordertab);
        }

        // GET: OrderTabController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var respose = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
            var find = _context.StatusTabs.ToList();
            OrdertabVM orderTVM=new OrdertabVM();
            if (id == null)
            {
                return View(List);
            }
            var order = await _context.OrderTabs.FindAsync(id);
            if (order==null)
            {
                return View(List);
            }
            orderTVM.OrderNumber = order.OrderNumber;
            orderTVM.CustomerId = order.CustomerId;
            orderTVM.OrderDate = order.OrderDate;
            orderTVM.SubTotal = order.SubTotal;
            orderTVM.Discount = order.Discount;
            orderTVM.ShippingFee = order.ShippingFee;
            orderTVM.NetAmount = order.NetAmount;
            orderTVM.StatusId = order.StatusId;
            
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(respose, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            var itemsJson = HttpContext.Session.GetString("order");
            var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrdertabVM>>(itemsJson) : new List<OrdertabVM>();
            return View("Index", orderTVM);
        }

        // POST: OrderTabController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderTabController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Order = await _context.OrderTabs .FirstOrDefaultAsync(m => m.OrderId == id);
            if (Order == null)
            {
                return NotFound();
            }
            return View("Details", Order);
        }

        // POST: OrderTabController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
