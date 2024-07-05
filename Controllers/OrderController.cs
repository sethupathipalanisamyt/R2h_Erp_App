using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using R2h_Erp_App.DbModels;
using R2h_Erp_App.Models;

namespace R2h_Erp_App.Controllers
{
    public class OrderController : Controller
    {
        private readonly R2hErpDbContext _context;

        public OrderController(R2hErpDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
           
            return View( await _context.Orders.Where(x => !x.Isdeleted).Include(o => o.Customers).Include(o => o.Product).ToListAsync());
            
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var result=_context.Customers.ToList().Where(x=> !x.IsActive==false).Where(x=>!x.Isdeleted);
            var response=_context.Products.ToList().Where(x=>!x.IsActive==false).Where(x => !x.Isdeleted);
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomersId,ProductId,OrderDate,Quantity,Amount")] OrderVM orderVM)
        {
            Order order = new Order();

            if (orderVM !=null)
            {
                
                order.CustomersId= orderVM.CustomersId;
                order.ProductId= orderVM.ProductId;
                order.Quantity = orderVM.Quantity;
                order.Amount = orderVM.Amount;
                var totalamont= order.Amount * order.Quantity;
                order.TotalAmount = totalamont;
                order.OrderDate = orderVM.OrderDate;
                order.CreatedOn=DateTime.Now;
                order.UpdateedOn = null;
                order.Isdeleted = false;
                _context.Add(order);
                 _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "CustomersId", "CustomersId", order.CustomersId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", order.ProductId);
            return View(orderVM);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "CustomersId", "CustomersId", order.CustomersId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", order.ProductId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Quantity,Amount,TotalAmound")] OrderVM orderVM)
        {
            
            if (ModelState.IsValid)
            {

                Order order = _context.Orders.Find(id);
                order.Quantity=orderVM.Quantity;
                order.Amount=orderVM.Amount;
                var totalamont = order.Amount * order.Quantity;
                order.TotalAmount = totalamont;
                order.UpdateedOn = DateTime.Now;



                _context.Update(order);
                await _context.SaveChangesAsync();
                
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "CustomersId", "CustomersId", orderVM.CustomersId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", orderVM.ProductId);
            return View(orderVM);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customers)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Remove(Order id)
        {
            var order = await _context.Orders.FindAsync(id.OrderId);
            if (order != null)
            {
                order.Isdeleted = true;
                _context.Orders.Update(order);
                _context.SaveChanges();
               
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
