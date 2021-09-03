using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class HomeVM
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
