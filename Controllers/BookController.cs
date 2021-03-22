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
    public class BookController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> data_list = _db.Book.Include(u=>u.Category);
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
            return View(new Book());
            else
            {
                var obj = _db.Book.Find(id);
                if (id != null)
                    return View(obj);
                else
                    return NotFound();
            }
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert( Book Book)
        {

            var files = HttpContext.Request.Form.Files;
            string rootpath = _webHostEnvironment.WebRootPath;
            string filename = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);
            string path = Path.Combine(rootpath + "\\Image\\", filename + extension);
            if (Book.book_id==0)
            {
                //create
               
                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(FileStream);
                }
                Book.book_image = filename + extension;
                _db.Add(Book);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            {
                //updating
                var objFromDb = _db.Book.AsNoTracking().FirstOrDefault(u => u.book_id == Book.book_id);
                
                if (objFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                    var imagepath = rootpath + WC.ImagePath + objFromDb.book_image;
                    System.IO.File.Delete(imagepath);
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }
                    Book.book_image = filename + extension;
                    _db.Update(Book);
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
                var obj = _db.Book.Find(id);
                return View(obj);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Book obj)
        {
            var objFromDb = _db.Book.AsNoTracking().FirstOrDefault(u => u.book_id == obj.book_id);
            if (objFromDb == null)
            {
                return NotFound();
            }
            else
            {
                string rootpath = _webHostEnvironment.WebRootPath;
                string path = rootpath + WC.ImagePath;
                System.IO.File.Delete( path+ objFromDb.book_image);
                _db.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
