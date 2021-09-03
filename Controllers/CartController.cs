using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageCRUD.Models;
using ImageCRUD.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageCRUD.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
            

        }
        public IActionResult Index()
        {
            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.ShoppingCart) != null)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.ShoppingCart);

            }
            List<int> ProdInCart = ShoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> BookList = _db.Product.Include(u => u.Category).Where(b => ProdInCart.Contains(b.product_id));
            return View(BookList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Summary()
        {
            var email = HttpContext.User.Identity.Name;

            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.ShoppingCart) != null)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.ShoppingCart);

            }
            List<int> ProdInCart = ShoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> ProductEnum = _db.Product.Include(u => u.Category).Where(b => ProdInCart.Contains(b.product_id));
            List < Product > ProductList= ProductEnum.ToList();
            ProductUserVM productUserVM = new ProductUserVM()
            {
                BookList = ProductList,
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Email == email)

            };
            return View(productUserVM);
        }



        public IActionResult Remove(int id)
        {
            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.ShoppingCart) != null)
            {
                ShoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.ShoppingCart);

            }
            var productToRemove = ShoppingCartList.FirstOrDefault(u => u.ProductId == id);
            ShoppingCartList.Remove(productToRemove);
            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.ShoppingCart, ShoppingCartList);
            return RedirectToAction(nameof(Index));
        }


       

     }

}
