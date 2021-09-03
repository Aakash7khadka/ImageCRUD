using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class Product
    {
        [Key]
       public  int product_id { get; set; }
       public string product_name { get; set; }
        
       public float product_price { get; set; }
       public string product_image { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
