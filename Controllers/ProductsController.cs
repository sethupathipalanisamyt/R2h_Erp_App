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
    public class ProductsController : Controller
    {
        private readonly R2hErpDbContext _context;

        public ProductsController(R2hErpDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Where(x => !x.Isdeleted).Where(x=>x.IsActive).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductsId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Code,IsActive")] ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                Product product =new Product();
                product.Name = productVM.Name;
                product.Code=productVM.Code;
                product.IsActive = true;
                product.CreatedOn = DateTime.Now;
                product.UpdateedOn = null;
                product.Isdeleted=false;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ProductVM productVM=new ProductVM();
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            productVM.Name = product.Name;
            productVM.Code= product.Code;
            productVM.IsActive = product.IsActive;
            productVM.ProductsId= product.ProductsId;

            return View(productVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductsId,Name,Code,IsActive")] ProductVM productVM)
        {
            
            if (ModelState.IsValid)
            {
                Product product = _context.Products.Find(id)!;
                product.Name=productVM.Name;
                product.Code=productVM.Code;
                product.IsActive = productVM.IsActive;
                product.UpdateedOn= DateTime.Now;
                _context.Update(product);
                await  _context.SaveChangesAsync();
                
               
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductsId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Product id)
        {
            var product = await _context.Products.FindAsync(id.ProductsId);
            if (product != null)
            {
                id.Isdeleted = true;  
                _context.Products.Update(product);
                _context.SaveChanges();
                
               
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductsId == id);
        }
    }
}
