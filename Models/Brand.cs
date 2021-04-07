using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Brand
    {
        public Brand()
        {
            Cars = new HashSet<Car>();
        }

        public int BrandId { get; set; }
        [Display(Name = "Бренд")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string BrandName { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }

        [Display(Name = "Країна")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
