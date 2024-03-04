using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_5.Models
{
    public class Product
    {
        public string PathToPhoto { get; set; }

        [Required(ErrorMessage = "Поле 'Короткое название' пустое. Введите короткое название")]
        public string NameShort { get; set; }

        [Required(ErrorMessage = "Поле 'Полное название' пустое. Введите полное название")]
        public string NameLong { get; set; }

        [Required(ErrorMessage = "Поле 'Категория' пустое. Выберите категорию")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Поле 'Стоимость' пустое. Введите стоимость")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Поле 'Количество' пустое. Введите количество")]
        public double Quantity { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Поле 'Рейтинг' пустое. Введите рейтинг")]
        [Range (1.0,5.0, ErrorMessage = "Введите рейтинг от 1 до 5")]
        public double Score { get; set; }

        [Required(ErrorMessage = "Поле 'Страна-производитель' пустое. Выберите страну")]
        public string Country { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsNotAvailable { get; set; }

    }
}
