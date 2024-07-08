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
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext _context;

        public OrderTabController(R2hErpDbContext context)
        {
            _context = context;
        }

        // GET: OrderTab
        public async Task<IActionResult> Index()
        {
            var r2hErpDbContext = _context.OrderTabs.Include(o => o.Customer);
            return View(await r2hErpDbContext.ToListAsync());
        }

        // GET: OrderTab/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTab = await _context.OrderTabs
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderTab == null)
            {
                return NotFound();
            }

            return View(orderTab);
        }

        // GET: OrderTab/Create
        public IActionResult Create()
        {
            var result = _context.Customers.ToList().Where(x => !x.IsActive == false).Where(x => !x.Isdeleted);
            var response = _context.Products.ToList().Where(x => !x.IsActive == false).Where(x => !x.Isdeleted);
            ViewBag.CustomerId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            return View("Create");
        }

        // POST: OrderTab/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNumber,CustomerId,OrderDate,SubTotal,Discount,ShippingFee,NetAmount,StatusId")] OrderTabVM orderTabVM)
        {
            OrderTab ordertab=new OrderTab();
            if (ModelState.IsValid)
            {
                ordertab.OrderNumber= orderTabVM.OrderNumber;
                ordertab.CustomerId= orderTabVM.CustomerId;
                ordertab.OrderDate=DateTime.Now;
                ordertab.SubTotal= orderTabVM.SubTotal;
                ordertab.Discount= orderTabVM.Discount;
                ordertab.ShippingFee= orderTabVM.ShippingFee;
                ordertab.NetAmount= orderTabVM.NetAmount;
                ordertab.StatusId= orderTabVM.StatusId;
                _context.Add(ordertab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CustomerId = new SelectList(_context.Customers, "CustomersId", "Name", orderTabVM.CustomerId);
            return View(orderTabVM);
        }

        // GET: OrderTab/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var result = _context.OrderTabs.Find(id);
            OrderTabVM orderTabVM = new OrderTabVM();
            if (result!=null)
            {
                orderTabVM.OrderNumber = result.OrderNumber;
                orderTabVM.OrderId= result.OrderId;
                orderTabVM.OrderDate = result.OrderDate;
                orderTabVM.CustomerId = result.CustomerId;
                orderTabVM.SubTotal = result.SubTotal;
                orderTabVM.Discount = result.Discount;
                orderTabVM.ShippingFee = result.ShippingFee;
                orderTabVM.NetAmount=result.NetAmount;
                orderTabVM.StatusId = result.StatusId;
                var find = _context.Customers.ToList();
                ViewBag.CustomerId = new SelectList(find, "CustomersId", "Name", orderTabVM.CustomerId);
                return View(orderTabVM);
            }
           
            return View();
        }

        // POST: OrderTab/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderTabVM orderTabVM)
        {
            if (ModelState.IsValid)
            {
                OrderTab ordertab = _context.OrderTabs.Find(id);
                ordertab.OrderDate= orderTabVM.OrderDate;
                ordertab.SubTotal= orderTabVM.SubTotal;
                ordertab.Discount= orderTabVM.Discount;
                ordertab.ShippingFee= orderTabVM.ShippingFee;
                ordertab.NetAmount= orderTabVM.NetAmount;
                ordertab.StatusId= orderTabVM.StatusId;
                ordertab.OrderNumber= orderTabVM.OrderNumber;

                
                    _context.Update(ordertab);
                await _context.SaveChangesAsync();          
                return RedirectToAction(nameof(Index));
            }
          
            return View(orderTabVM);
        }

        // GET: OrderTab/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTab = await _context.OrderTabs
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderTab == null)
            {
                return NotFound();
            }

            return View(orderTab);
        }

        // POST: OrderTab/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var orderTab = await _context.OrderTabs.FindAsync(id);
            if (orderTab != null)
            {
                _context.OrderTabs.Remove(orderTab);
            }

           
            return RedirectToAction(nameof(Index));
        }

        private bool OrderTabExists(int id)
        {
            return _context.OrderTabs.Any(e => e.OrderId == id);
        }
    }
}
