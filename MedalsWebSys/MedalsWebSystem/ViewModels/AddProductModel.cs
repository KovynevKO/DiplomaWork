using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class AddProductModel
    {
        
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Название должно быть не более 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указана категория")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Не указана страна")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Не указан год")]
        [Range(0, 3000, ErrorMessage = "Недопустимое значение")]
        public int? Year { get; set; }
        [Required(ErrorMessage = "Не указан диаметр")]
        [Range(0, 3000, ErrorMessage = "Недопустимое значение")]
        public float? Diameter { get; set; }
        [Required(ErrorMessage = "Не указан вес")]
        [Range(0, 100000, ErrorMessage = "Недопустимое значение")]
        public float? Weight { get; set; }
        [Required(ErrorMessage = "Не указан материал")]
        public string Material { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
