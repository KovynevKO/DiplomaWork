using MedalsWebSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class CatalogViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterInformation FilterInformation { get; set; }
        public SortingInformation SortingInformation { get; set; }

        public User User { get; set; } = null;
    }
}
