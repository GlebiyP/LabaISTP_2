using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Color
    {
        public Color()
        {
            Cars = new HashSet<Car>();
        }

        public int ColorId { get; set; }
        [Display(Name = "Колір")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string ColorName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
