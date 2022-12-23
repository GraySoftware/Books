using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        // here we are creating dropdown lists populated with
        // information from the Category table is passed using ViewBag
        // information from the CoverType table is passed using ViewData
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }

    }
}
