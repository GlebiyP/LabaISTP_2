using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace LabaISTP_2
{
    public partial class CarSale
    {
        public int CarSaleId { get; set; }
        [Display(Name = "Покупець")]
        public int CustomerId { get; set; }
        [Display(Name = "Продавець")]
        public int SellerId { get; set; }
        [Display(Name = "Модель")]
        public int CarId { get; set; }
        [Display(Name = "Ціна, $")]
        [Required(ErrorMessage = "Поле не може бути пустим!")]
        [Range(0, 1000000000, ErrorMessage = "Неприпустиме значення")]
        public double Price { get; set; }

        [Display(Name = "Модель")]
        public virtual Car Car { get; set; }
        [Display(Name = "Покупець")]
        public virtual Customer Customer { get; set; }
        [Display(Name = "Продавець")]
        public virtual Seller Seller { get; set; }
    }
}
