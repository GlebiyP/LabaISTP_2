using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class Car
    {
        public Car()
        {
            CarSales = new HashSet<CarSale>();
        }

        public int CarId { get; set; }
        [Display(Name = "Бренд")]
        public int BrandId { get; set; }
        [Display(Name = "Рік випуску")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        [Range(1940, 2021, ErrorMessage = "Значення повинно бути у діапазоні вiд 1940 до 2021!")]
        public string CarYear { get; set; }
        [Display(Name = "Кузов")]
        public int BodyId { get; set; }
        [Display(Name = "VIN-код")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        [RegularExpression(@"[A-Z]{2}[0-9]{8}", ErrorMessage = "Невірний формат")]
        public string Vin { get; set; }
        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        public string Model { get; set; }
        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Display(Name = "Кузов")]
        public virtual Body Body { get; set; }
        [Display(Name = "Бренд")]
        public virtual Brand Brand { get; set; }
        [Display(Name = "Колір")]
        public virtual Color Color { get; set; }
        public virtual ICollection<CarSale> CarSales { get; set; }
    }
}
