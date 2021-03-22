using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string category_name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
