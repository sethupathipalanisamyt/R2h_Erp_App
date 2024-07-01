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
            var r2hErpDbContext = _context.Orders.Include(o => o.Customers).Include(o => o.Product);
            return View(await r2hErpDbContext.ToListAsync());
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
            var result=_context.Customers.ToList();
            var response=_context.Products.ToList();
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

            if (ModelState.IsValid)
            {
                
                order.CustomersId= orderVM.CustomersId;
                order.ProductId= orderVM.ProductId;
                order.OrderDate = orderVM.OrderDate;
                order.Quantity = orderVM.Quantity;
                order.Amount = orderVM.Amount;
                order.CreatedOn=DateTime.Now;
                order.UpdateedOn = null;
                order.Isdeleted = false;
                _context.Add(order);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomersId,ProductId,OrderDate,Quantity,Amount,CreatedOn,UpdateedOn,Isdeleted")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "CustomersId", "CustomersId", order.CustomersId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductsId", "ProductsId", order.ProductId);
            return View(order);
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
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
