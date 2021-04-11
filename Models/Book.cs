using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCRUD.Models
{
    public class Book
    {
        [Key]
       public  int book_id { get; set; }
       public string book_name { get; set; }
        
       public float book_price { get; set; }
       public string book_image { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
