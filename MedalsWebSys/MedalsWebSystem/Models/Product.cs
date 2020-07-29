using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public int? Year { get; set; }
        public string Material { get; set; }
        public float? Diameter { get; set; }
        public float? Weight { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public byte[] minPhoto { get; set; }
        public bool isMainCatalog { get; set; }
        public DateTime DateTime { get; set; }
        public User UserAdder { get; set; }
        public List<UserProduct> UserProducts { get; set; }
        public Product()
        {
            UserProducts = new List<UserProduct>();
        }
    }
}
