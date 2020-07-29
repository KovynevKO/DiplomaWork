using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class FilterInformation
    {
        public string name { get; set; }
        public string category { get; set; }
        public int? minYear { get; set; }
        public int? maxYear { get; set; }
        public float? minWeight { get; set; }
        public float? maxWeight { get; set; }
        public float? minDiameter { get; set; }
        public float? maxDiameter { get; set; }
        public string country { get; set; }
        public string material { get; set; }
    }
}
