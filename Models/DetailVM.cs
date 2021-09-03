using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
