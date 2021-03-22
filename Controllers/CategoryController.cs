using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageCRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageCRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var obj = _db.Category;
            return View(obj);
        }
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            _db.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            if(id==0 )
            {
                return NotFound();
            }
            else
            {
                var objFromDb = _db.Category.Find(id);
                if(objFromDb==null)
                {
                    return NotFound();
                }
                else
                {
                    return View(objFromDb);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            _db.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var objFromDb = _db.Category.Find(id);
                if (objFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(objFromDb);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
