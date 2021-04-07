using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LabaISTP_2
{
    public class CarsListViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public SelectList Bodies { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Colors { get; set; }
        public SelectList Years { get; set; }
    }
}
