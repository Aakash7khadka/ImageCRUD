using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class ProductUserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<Product> BookList { get; set; }

        public ProductUserVM()
        {
            BookList = new List<Product>();
        }
    }
}
