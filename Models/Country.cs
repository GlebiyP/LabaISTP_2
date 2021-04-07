using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Country
    {
        public Country()
        {
            Brands = new HashSet<Brand>();
            Cities = new HashSet<City>();
        }

        public int CountryId { get; set; }
        [Display(Name = "Країна")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string CountryName { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
