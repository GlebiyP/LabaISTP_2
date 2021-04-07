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
    public class BodiesController : Controller
    {
        private readonly CarSalesContext _context;

        public BodiesController(CarSalesContext context)
        {
            _context = context;
        }

        // GET: Bodies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bodies.ToListAsync());
        }

        // GET: Bodies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyId == id);
            if (body == null)
            {
                return NotFound();
            }

            //return View(body);
            return RedirectToAction("IndexByBody", "Cars", new { id = body.BodyId, name = body.BodyName });
        }

        // GET: Bodies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bodies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BodyId,BodyName")] Body body)
        {
            if (ModelState.IsValid)
            {
                _context.Add(body);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(body);
        }

        // GET: Bodies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies.FindAsync(id);
            if (body == null)
            {
                return NotFound();
            }
            return View(body);
        }

        // POST: Bodies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BodyId,BodyName")] Body body)
        {
            if (id != body.BodyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(body);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyExists(body.BodyId))
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
            return View(body);
        }

        // GET: Bodies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var body = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyId == id);
            if (body == null)
            {
                return NotFound();
            }

            return View(body);
        }

        // POST: Bodies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var body = await _context.Bodies.FindAsync(id);
            _context.Bodies.Remove(body);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyExists(int id)
        {
            return _context.Bodies.Any(e => e.BodyId == id);
        }
    }
}
