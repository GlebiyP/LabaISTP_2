using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Http;
using LabaISTP_2;
using Microsoft.AspNetCore.Authorization;

namespace LabaISTP_2.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarSalesContext _context;

        public CarsController(CarSalesContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(int? body, int? brand, int? color, string year)
        {
            IQueryable<Car> cars = _context.Cars.Include(c => c.Body).Include(c => c.Brand).Include(c => c.Color);
            if (body != null && body != 0)
            {
                cars = cars.Where(c => c.BodyId == body);
            }
            if (brand != null && brand != 0)
            {
                cars = cars.Where(c => c.BrandId == brand);
            }
            if (color != null && color != 0)
            {
                cars = cars.Where(c => c.ColorId == color);
            }
            if (!String.IsNullOrEmpty(year) && !year.Equals(""))
            {
                cars = cars.Where(c => c.CarYear == year);
            }

            List<Body> bodies = _context.Bodies.ToList();
            List<Brand> brands = _context.Brands.ToList();
            List<Color> colors = _context.Colors.ToList();
            List<string> years = _context.Cars.Select(c => c.CarYear).Distinct().ToList();

            bodies.Insert(0, new Body { BodyName = "", BodyId = 0 });
            brands.Insert(0, new Brand { BrandName = "", BrandId = 0 });
            colors.Insert(0, new Color { ColorName = "", ColorId = 0 });
            years.Insert(0, "");

            CarsListViewModel clvm = new CarsListViewModel
            {
                Cars = cars.ToList(),
                Bodies = new SelectList(bodies, "BodyId", "BodyName"),
                Brands = new SelectList(brands, "BrandId", "BrandName"),
                Colors = new SelectList(colors, "ColorId", "ColorName"),
                Years = new SelectList(years, "CarYear")
            };
            return View(clvm);
            //var carSalesContext = _context.Cars.Include(c => c.Body).Include(c => c.Brand).Include(c => c.Color);
            //return View(await carSalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexByBrand(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Brands", "Index");
            ViewBag.BrandId = id;
            ViewBag.BrandName = name;
            var carsByBrand = _context.Cars.Where(c => c.BrandId == id).Include(c => c.Brand).Include(c => c.Body).Include(c => c.Color);

            return View(await carsByBrand.ToListAsync());
        }

        public async Task<IActionResult> IndexByBody(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Bodies", "Index");
            ViewBag.BodyId = id;
            ViewBag.BodyName = name;
            var carsByBody = _context.Cars.Where(c => c.BodyId == id).Include(c => c.Body).Include(c => c.Brand).Include(c => c.Color);

            return View(await carsByBody.ToListAsync());
        }

        public async Task<IActionResult> IndexByColor(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Colors", "Index");
            ViewBag.ColorId = id;
            ViewBag.ColorName = name;
            var carsByColor = _context.Cars.Where(c => c.ColorId == id).Include(c => c.Color).Include(c => c.Brand).Include(c => c.Body);

            return View(await carsByColor.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Body)
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName");
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,BrandId,CarYear,BodyId,Vin,Model,ColorId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName", car.BodyId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", car.BrandId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            return View(car);
        }

        public IActionResult CreateByBrand(int brandId)
        {
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewBag.BrandId = brandId;
            ViewBag.BrandName = _context.Brands.Where(b => b.BrandId == brandId).FirstOrDefault().BrandName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByBrand(int brandId, [Bind("CarId,BrandId,CarYear,BodyId,Vin,Model,ColorId")] Car car)
        {
            car.BrandId = brandId;
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexByBrand", "Cars", new { id = brandId, name = _context.Brands.Where(b => b.BrandId == brandId).FirstOrDefault().BrandName });
            }
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName", car.BodyId);
            //ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", car.BrandId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            //return View(car);
            return RedirectToAction("IndexByBrand", "Cars", new { id = brandId, name = _context.Brands.Where(b => b.BrandId == brandId).FirstOrDefault().BrandName });
        }

        public IActionResult CreateByBody(int bodyId)
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewBag.BodyId = bodyId;
            ViewBag.BodyName = _context.Bodies.Where(b => b.BodyId == bodyId).FirstOrDefault().BodyName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByBody(int bodyId, [Bind("CarId,BrandId,CarYear,BodyId,Vin,Model,ColorId")] Car car)
        {
            car.BodyId = bodyId;
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexByBody", "Cars", new { id = bodyId, name = _context.Bodies.Where(b => b.BodyId == bodyId).FirstOrDefault().BodyName });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", car.BrandId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            //return View(car);
            return RedirectToAction("IndexByBody", "Cars", new { id = bodyId, name = _context.Bodies.Where(b => b.BodyId == bodyId).FirstOrDefault().BodyName });
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName", car.BodyId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", car.BrandId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,BrandId,CarYear,BodyId,Vin,Model,ColorId")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
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
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "BodyName", car.BodyId);
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", car.BrandId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", car.ColorId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Body)
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Brand newBrand;
                                Country newCountry;
                                var b = (from br in _context.Brands
                                         where br.BrandName.Contains(worksheet.Name)
                                         select br).ToList();
                                var c = (from cn in _context.Countries
                                         where cn.CountryName.Contains(worksheet.Cell(1, 1).Value.ToString())
                                         select cn).ToList();
                                if (b.Count > 0)
                                {
                                    newBrand = b[0];
                                }
                                else
                                {
                                    if (c.Count > 0)
                                    {
                                        newCountry = c[0];
                                    }
                                    else
                                    {
                                        newCountry = new Country();
                                        newCountry.CountryName = worksheet.Cell(1, 1).Value.ToString();
                                        _context.Countries.Add(newCountry);
                                    }
                                    newBrand = new Brand();
                                    newBrand.BrandName = worksheet.Name;
                                    newBrand.Country = newCountry;
                                    _context.Brands.Add(newBrand);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Car car = new Car();
                                        car.Brand = newBrand;
                                        car.Model = row.Cell(2).Value.ToString();
                                        car.CarYear = row.Cell(4).Value.ToString();
                                        car.Vin = row.Cell(5).Value.ToString();
                                        _context.Cars.Add(car);

                                        if (row.Cell(3).Value.ToString().Length > 0)
                                        {
                                            Body body;
                                            var bd = (from bod in _context.Bodies
                                                      where
                                                      bod.BodyName.Contains(row.Cell(3).Value.ToString())
                                                      select bod).ToList();
                                            if (bd.Count > 0)
                                            {
                                                body = bd[0];
                                            }
                                            else
                                            {
                                                body = new Body();
                                                body.BodyName = row.Cell(3).Value.ToString();
                                                _context.Add(body);
                                            }
                                            car.Body = body;
                                        }

                                        if (row.Cell(6).Value.ToString().Length > 0)
                                        {
                                            Color color;
                                            var cl = (from col in _context.Colors
                                                      where
                                                      col.ColorName.Contains(row.Cell(6).Value.ToString())
                                                      select col).ToList();
                                            if (cl.Count > 0)
                                            {
                                                color = cl[0];
                                            }
                                            else
                                            {
                                                color = new Color();
                                                color.ColorName = row.Cell(6).Value.ToString();
                                                _context.Add(color);
                                            }
                                            car.Color = color;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export(int? body, int? brand, int? color, string year)
        {
            IQueryable<Car> cars = _context.Cars.Include(c => c.Body).Include(c => c.Brand).Include(c => c.Color);
            if (body != null && body != 0)
            {
                cars = cars.Where(c => c.BodyId == body);
            }
            if (brand != null && brand != 0)
            {
                cars = cars.Where(c => c.BrandId == brand);
            }
            if (color != null && color != 0)
            {
                cars = cars.Where(c => c.ColorId == color);
            }
            if (!String.IsNullOrEmpty(year) && !year.Equals(""))
            {
                cars = cars.Where(c => c.CarYear == year);
            }

            var brands = cars.Select(c => c.Brand).Distinct().ToList();

            foreach (var b in brands)
            {
                foreach (var c in cars)
                {
                    if (c.Brand.Equals(b))
                    {
                        b.Cars.Add(c);
                    }
                }
            }

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                foreach (var b in brands)
                {
                    var worksheet = workbook.Worksheets.Add(b.BrandName);

                    worksheet.Cell("A1").Value = "Модель";
                    worksheet.Cell("B1").Value = "Кузов";
                    worksheet.Cell("C1").Value = "Рік випуску";
                    worksheet.Cell("D1").Value = "VIN-код";
                    worksheet.Cell("E1").Value = "Колір";

                    for (int i = 1; i < 6; i++)
                    {
                        worksheet.Cell(1, i).Style.Fill.SetBackgroundColor(XLColor.FromArgb(0x70, 0xad, 0x47));
                    }

                    int j = 0;
                    foreach (var c in b.Cars)
                    {
                        worksheet.Cell(j + 2, 1).Value = c.Model;
                        worksheet.Cell(j + 2, 2).Value = c.Body.BodyName;
                        worksheet.Cell(j + 2, 3).Value = c.CarYear;
                        worksheet.Cell(j + 2, 4).Value = c.Vin;
                        worksheet.Cell(j + 2, 5).Value = c.Color.ColorName;
                        j++;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"cars_libraby_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
