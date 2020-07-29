using MedalsWebSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class ProductInfoViewModel
    {
        public List<User> Users { get; set; }
        public Product Product { get; set; }
    }
}
