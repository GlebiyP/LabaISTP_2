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
    public class ColorsChartsController : ControllerBase
    {
        private readonly CarSalesContext _context;

        public ColorsChartsController(CarSalesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var colors = _context.Colors.Include(c => c.Cars).ToList();

            List<object> colorCar = new List<object>();

            colorCar.Add(new[] { "Колір", "Кількість авто" });

            foreach (var c in colors)
            {
                colorCar.Add(new object[] { c.ColorName, c.Cars.Count() });
            }
            return new JsonResult(colorCar);
        }
    }
}
