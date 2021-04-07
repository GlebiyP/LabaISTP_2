using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabaISTP_2;
using Microsoft.AspNetCore.Authorization;

namespace LabaISTP_2.Controllers
{
    public class CarSalesController : Controller
    {
        private readonly CarSalesContext _context;

        public CarSalesController(CarSalesContext context)
        {
            _context = context;
        }

        // GET: CarSales
        public async Task<IActionResult> Index()
        {
            var carSalesContext = _context.CarSales.Include(c => c.Car).Include(c => c.Customer).Include(c => c.Seller).Include(c => c.Car.Brand);
            return View(await carSalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexBySeller(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Sellers", "Index");
            ViewBag.SellerId = id;
            ViewBag.SellerName = name;
            var carSalesBySeller = _context.CarSales.Where(c => c.SellerId == id).Include(c => c.Seller).Include(c => c.Car.Brand).Include(c => c.Customer);

            return View(await carSalesBySeller.ToListAsync());
        }

        public async Task<IActionResult> IndexByCustomer(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Customers", "Index");
            ViewBag.CustomerId = id;
            ViewBag.CustomerName = name;
            var carSalesByCustomer = _context.CarSales.Where(c => c.CustomerId == id).Include(c => c.Customer).Include(c => c.Seller).Include(c => c.Car.Brand);

            return View(await carSalesByCustomer.ToListAsync());
        }

        // GET: CarSales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales
                .Include(c => c.Car)
                .Include(c => c.Car.Brand)
                .Include(c => c.Customer)
                .Include(c => c.Seller)
                .FirstOrDefaultAsync(m => m.CarSaleId == id);
            if (carSale == null)
            {
                return NotFound();
            }

            return View(carSale);
        }

        // GET: CarSales/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "Model");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName");
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "SellerName");
            return View();
        }

        // POST: CarSales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarSaleId,CustomerId,SellerId,CarId,Price")] CarSale carSale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carSale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "Model", carSale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", carSale.CustomerId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "SellerName", carSale.SellerId);
            return View(carSale);
        }

        // GET: CarSales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales.FindAsync(id);
            if (carSale == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "CarYear", carSale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", carSale.CustomerId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "SellerName", carSale.SellerId);
            return View(carSale);
        }

        // POST: CarSales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarSaleId,CustomerId,SellerId,CarId,Price")] CarSale carSale)
        {
            if (id != carSale.CarSaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarSaleExists(carSale.CarSaleId))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "CarYear", carSale.CarId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerName", carSale.CustomerId);
            ViewData["SellerId"] = new SelectList(_context.Sellers, "SellerId", "SellerName", carSale.SellerId);
            return View(carSale);
        }

        // GET: CarSales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSale = await _context.CarSales
                .Include(c => c.Car)
                .Include(c => c.Customer)
                .Include(c => c.Seller)
                .FirstOrDefaultAsync(m => m.CarSaleId == id);
            if (carSale == null)
            {
                return NotFound();
            }

            return View(carSale);
        }

        // POST: CarSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carSale = await _context.CarSales.FindAsync(id);
            _context.CarSales.Remove(carSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarSaleExists(int id)
        {
            return _context.CarSales.Any(e => e.CarSaleId == id);
        }
    }
}