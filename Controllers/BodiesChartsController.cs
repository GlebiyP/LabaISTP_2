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
    public class BodiesChartsController : ControllerBase
    {
        private readonly CarSalesContext _context;

        public BodiesChartsController(CarSalesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var bodies = _context.Bodies.Include(b => b.Cars).ToList();

            List<object> bodyCar = new List<object>();

            bodyCar.Add(new[] { "Кузов", "Кількість авто" });

            foreach (var b in bodies)
            {
                bodyCar.Add(new object[] { b.BodyName, b.Cars.Count() });
            }
            return new JsonResult(bodyCar);
        }
    }
}

