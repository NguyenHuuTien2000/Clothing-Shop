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
        private string UserID;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
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

        public IActionResult CartPage()
        {
            UserID = _userManager.GetUserId(User);

            var cart = _context.Carts
                .Include(d => d.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.UserId == UserID);
            if (cart == null || cart.CartItems == null)
            {
                return View();
            }
            return View(cart.CartItems.ToList());
        }

        public IActionResult AddToCart(int? pid)
        {
            UserID = _userManager.GetUserId(User);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(x => x.UserId == UserID);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = UserID
                };
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == pid);
            if (product == null)
            {
                return NotFound();
            }
            var cartItem = new CartItem();
            cartItem.CartID = cart.Id;
            cartItem.ProductID = product.Id;
            cartItem.Product = product;
            cartItem.MyCart = cart;
            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }
            cart.CartItems.Add(cartItem);
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(CartPage));
        }

        public IActionResult RemoveItem(int? id)
        {
            UserID = _userManager.GetUserId(User);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .Single(x => x.UserId == UserID);

            if (cart.CartItems == null)
            {
                return View(cart);
            }

            var itemList = cart.CartItems.ToList();
            foreach (var item in itemList)
            {
                if (item.Id == id)
                {
                    cart.CartItems.Remove(item);
                }
            }
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(CartPage));
        }

        public IActionResult RemoveAllItemCart()
        {
            var cart = _context.Carts
                .Include(d => d.CartItems)
                .Single();
            if (cart == null || cart.CartItems == null)
            {
                return View();
            }

            cart.CartItems.Clear();
            _context.Carts.Update(cart);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(ConfirmOrder));
        }


        public IActionResult ConfirmOrder()
        {
            UserID = _userManager.GetUserId(User);
            var cart = _context.Carts
                 .Include(d => d.SumPayment)
                .Include(c => c.CartItems)
                .ThenInclude(b => b.Product)
                .Single(x => x.UserId == UserID);
            var history = _context.History.Single(x => x.UserId == UserID);
            foreach (CartItem c in cart.CartItems)
            {
                var historyStuff = new HistoryItems();
                historyStuff.HistoryID = history.Id;
                historyStuff.ProductId = c.ProductID;
                historyStuff.Product = c.Product;
                history.HistoryItems.Add(historyStuff);
                c.Product.Sell++;
                cart.SumPayment += c.Product.FinalPrice;
            }



            _context.Update(history);
            _context.Update(cart);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        

    }
}