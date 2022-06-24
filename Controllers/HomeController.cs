using Computer_Store.Data;
using Computer_Store.Models;
using Computer_Store.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Computer_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductComputer(int id)
        {
            var computer = _context.Computers
                .Include(c => c.Spec)
                .AsNoTracking()
                .Single(x => x.Id == id);
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        public IActionResult ComputerPage(string? type, string? brand)
        {
            var allComputers = _context.Computers.Include(c => c.Spec).AsNoTracking().ToList();

            if (type != null)
            {
                if (Enum.TryParse(type, out ComputerType reqType))
                {
                    allComputers = allComputers.Where(c => c.Type == reqType).ToList();
                }
            }

            if (brand != null)
            {
                if (Enum.TryParse(brand, out Brand reqBrand))
                {
                    allComputers = allComputers.Where(c => c.Brand == reqBrand).ToList();
                }
            }

            return View(allComputers);
        }

        public IActionResult ProductPart(int id)
        {
            var part = _context.Parts
                .AsNoTracking()
                .Single(x => x.Id == id);
            ViewData["Image"] = part.Image;
            return View(part);
        }

        public IActionResult PartPage(string? category, string? brand)
        {
            var allParts = _context.Parts.AsNoTracking().ToList();

            if (category != null)
            {
                if (Enum.TryParse(category, out PartCategory reqCategory))
                {
                    allParts = allParts.Where(c => c.Category == reqCategory).ToList();
                }
            }

            if (brand != null)
            {
                if (Enum.TryParse(brand, out Brand reqBrand))
                {
                    allParts = allParts.Where(c => c.Brand == reqBrand).ToList();
                }
            }

            return View(allParts);
        }

        public IActionResult AddToCart(string? uid, int? pid)
        {
            var cart = _context.Carts.Include(c => c.CartItems).Single(x => x.UserId == uid);
            var product = _context.Products.FirstOrDefault(p => p.Id == pid);
            var cartItem = new CartItems();
            cartItem.CartID = cart.Id;
            cartItem.ProductID = product.Id;
            cartItem.Product = product;
            cartItem.MyCart = cart;
            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItems>();
            }
            cart.CartItems.Add(cartItem);
            _context.Update(cart);
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CartPage(string? uid)
        {
            var cart = _context.Carts.Include(d => d.CartItems).FirstOrDefault(c => c.UserId == uid);
            cart.CartItems.ToList();
            return View(cart.CartItems.ToList());
        }

        public IActionResult removeAllItemCart(string? uid)
        {
            var cart = _context.Carts.Where(c => c.UserId == uid).Include(d => d.CartItems).FirstOrDefault();
            cart.CartItems.Clear();
            if (ModelState.IsValid)
            {
                _context.Update(cart);
                _context.SaveChangesAsync();
                return RedirectToPage("BuySuccess");
            }
            return View(cart);
        }

        public IActionResult removeItem(string? uid, int? pid)
        {
            var cart = _context.Carts.Single(x => x.UserId == uid);
            var product = _context.Products.FirstOrDefault(p => p.Id == pid);
            var cartItem = new CartItems();
            cartItem.CartID = cart.Id;
            cartItem.ProductID = product.Id;
            cartItem.Product = product;
            cart.CartItems.Remove(cartItem);

            _context.Update(cart);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            return View(cart);
        }

       
    }
}