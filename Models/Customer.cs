using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Customer
    {
        public Customer()
        {
            CarSales = new HashSet<CarSale>();
        }

        public int CustomerId { get; set; }
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string CustomerName { get; set; }
        [Display(Name = "Місто")]
        public int CityId { get; set; }

        [Display(Name = "Місто")]
        public virtual City City { get; set; }
        public virtual ICollection<CarSale> CarSales { get; set; }
    }
}
