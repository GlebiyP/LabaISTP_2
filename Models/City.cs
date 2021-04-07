using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class City
    {
        public City()
        {
            Customers = new HashSet<Customer>();
            Sellers = new HashSet<Seller>();
        }

        public int CityId { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }
        [Display(Name = "Місто")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string CityName { get; set; }

        [Display(Name = "Країна")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Seller> Sellers { get; set; }
    }
}
