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
    public class CustomersController : Controller
    {
        private readonly R2hErpDbContext _context;

        public CustomersController(R2hErpDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Customers.Where(x => !x.Isdeleted).ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomersId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Register()
        {
            return View("Create");
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,IsActive")] CustomerVM customerVM)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                customer.Name= customerVM.Name;
                customer.Email=customerVM.Email;
                customer.Phone = customerVM.Phone;
                customer.IsActive = true;
                customer.CreatedOn=DateTime.Now;
                customer.UpdateedOn = null;
                customer.Isdeleted = false;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerVM);
        }
        [HttpGet]
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            CustomerVM customervm = new CustomerVM();
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            customervm.Name = customer.Name;
            customervm.Email = customer.Email;
            customervm.Phone = customer.Phone;
            customervm.IsActive = customer.IsActive;
            customervm.CustomersId= customer.CustomersId;
            return View(customervm);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomersId,Name,Email,Phone,IsActive")] CustomerVM customerVM)
        {
            

            if (ModelState.IsValid)
            {
                Customer customer = _context.Customers.Find(customerVM.CustomersId)!;
                
                
                    customer.Name = customerVM.Name;
                    customer.Email = customerVM.Email;
                    customer.Phone = customerVM.Phone;
                    customer.IsActive = customerVM.IsActive;
                    customer.UpdateedOn= DateTime.Now;
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(customerVM);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomersId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Customer id)
        {
            var customer = await _context.Customers.FindAsync(id.CustomersId);
            if (customer != null)
            {
                customer.Isdeleted = true;
                _context.Customers.Update(customer);
                _context.SaveChanges();
               
            }

            _context.Remove(customer);    
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomersId == id);
        }
    }
}
