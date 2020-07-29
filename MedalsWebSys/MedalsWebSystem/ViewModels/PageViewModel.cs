using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedalsWebSystem.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public PageViewModel(int count, int page, int pageSize)
        {
            PageNumber = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
