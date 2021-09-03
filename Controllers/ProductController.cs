using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageCRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImageCRUD.Controllers
{
    public class ProductController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> data_list = _db.Product.Include(u=>u.Category);
            return View(data_list);
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> category_list = _db.Category.Select(i => new SelectListItem
            {
                Text = i.category_name,
                Value=i.id.ToString()
            }); 
            ViewBag.category_list =category_list;
            if(id==null)
            return View(new Product());
            else
            {
                var obj = _db.Product.Find(id);
                if (id != null)
                    return View(obj);
                else
                    return NotFound();
            }
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert( Product Product)
        {

            var files = HttpContext.Request.Form.Files;
            string rootpath = _webHostEnvironment.WebRootPath;
            string filename = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);
            string path = Path.Combine(rootpath + "\\Image\\", filename + extension);
            if (Product.product_id==0)
            {
                //create
               
                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(FileStream);
                }
                Product.product_image = filename + extension;
                _db.Add(Product);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                //updating
                var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.product_id == Product.product_id);
                
                if (objFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                    var imagepath = rootpath + WC.ImagePath + objFromDb.product_image;
                    System.IO.File.Delete(imagepath);
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }
                    Product.product_image = filename + extension;
                    _db.Update(Product);
                    _db.SaveChanges();
                    return RedirectToAction("Index");


                }

                

            }

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var obj = _db.Product.Find(id);
                return View(obj);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product obj)
        {
            var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.product_id == obj.product_id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            else
            {
                string rootpath = _webHostEnvironment.WebRootPath;
                string path = rootpath + WC.ImagePath;
                System.IO.File.Delete( path+ objFromDb.product_image);
                _db.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
