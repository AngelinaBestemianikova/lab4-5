using System;
using System.ComponentModel.DataAnnotations;

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

        public double Price { get; set; }

        public double Quantity { get; set; }

        public string Description { get; set; }

        [Range(1, 5, ErrorMessage = "Введите рейтинг от 1 до 5")]
        
        public double Score { get; set; }

        [Required(ErrorMessage = "Поле 'Страна-производитель' пустое. Выберите страну")]
        public string Country { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsNotAvailable { get; set; }
    }
}
