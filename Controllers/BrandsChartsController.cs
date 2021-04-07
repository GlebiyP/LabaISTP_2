using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LabaISTP_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsChartsController : ControllerBase
    {
        private readonly CarSalesContext _context;

        public BrandsChartsController(CarSalesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var brands = _context.Brands.Include(b => b.Cars).ToList();

            List<object> brandCar = new List<object>();

            brandCar.Add(new[] { "Бренд", "Кількість авто" });

            foreach (var b in brands)
            {
                brandCar.Add(new object[] { b.BrandName, b.Cars.Count() });
            }
            return new JsonResult(brandCar);
        }
    }
}