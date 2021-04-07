using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Body
    {
        public Body()
        {
            Cars = new HashSet<Car>();
        }

        public int BodyId { get; set; }
        [Display(Name = "Кузов")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string BodyName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
