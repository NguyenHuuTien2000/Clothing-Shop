using Computer_Store.Data;
using Computer_Store.Models;
using Computer_Store.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Pipelines;

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

        

        public IActionResult ComputerPage(string? type, string? brand, string searchString, string sortOrder)
        {
            ViewData["PriceSort"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_asc";
            ViewData["NameSort"] = searchString;


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

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allComputers = allComputers.Where(s => s.Name.ToLower().Contains(searchString)).ToList();
            }

            ViewData["CurrentSort"] = "Ascending";

            switch (sortOrder)
            {
                case "price_desc":
                    allComputers = allComputers.OrderByDescending(p => p.FinalPrice).ToList();
                    ViewData["CurrentSort"] = "Descending";
                    break;
                case "price_asc":
                    allComputers = allComputers.OrderBy(p => p.FinalPrice).ToList();
                    break;
                default:
                    allComputers = allComputers.OrderBy(p => p.FinalPrice).ToList();
                    break;
            }

            return View(allComputers);
        }

        public IActionResult PartPage(string? category, string? brand, string searchString, string sortOrder)
        {
            ViewData["PriceSort"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_asc";
            ViewData["NameSort"] = searchString;

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

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allParts = allParts.Where(s => s.Name.ToLower().Contains(searchString)).ToList();
            }

            ViewData["CurrentSort"] = "Ascending";

            switch (sortOrder)
            {
                case "price_desc":
                    allParts = allParts.OrderByDescending(p => p.FinalPrice).ToList();
                    ViewData["CurrentSort"] = "Descending";
                    break;
                case "price_asc":
                    allParts = allParts.OrderBy(p => p.FinalPrice).ToList();
                    break;
                default:
                    allParts = allParts.OrderBy(p => p.FinalPrice).ToList();
                    break;
            }

            return View(allParts);
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

        public IActionResult ProductPart(int id)
        {
            var part = _context.Parts
                .AsNoTracking()
                .Single(x => x.Id == id);
            ViewData["Image"] = part.Image;
            return View(part);
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
            double? currTotal = 0; 
            foreach (CartItem ci in cart.CartItems)
            {
                currTotal += ci.Product.FinalPrice;
            }
            ViewData["Total"] = string.Format("{0:n0}", currTotal);
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
            foreach (CartItem ci in cart.CartItems)
            {
                if (ci.ProductID == product.Id)
                {
                    return RedirectToAction(nameof(CartPage));
                }
            }
            var cartItem = new CartItem();
            cartItem.CartID = cart.Id;
            cartItem.ProductID = product.Id;
            cartItem.Product = product;
            cartItem.MyCart = cart;
            cartItem.Quantity = 1;
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

        //public IActionResult RemoveAllItemCart()
        //{
        //    var cart = _context.Carts
        //        .Include(d => d.CartItems)
        //        .Single();
        //    if (cart == null || cart.CartItems == null)
        //    {
        //        return View();
        //    }

        //    cart.CartItems.Clear();
        //    _context.Carts.Update(cart);
        //    _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ConfirmOrder));
        //}

        public IActionResult OrderPage()
        {
            UserID = _userManager.GetUserId(User);
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(b => b.Product)
                .Single(x => x.UserId == UserID);
            double? currTotal = 0;
            ApplicationUser user = _context.Users.Single(i => i.Id == UserID);
            double userDiscount = 0;
            string roles = _userManager.GetRolesAsync(user).ToString();
            if (roles.Contains("A_User"))
            {
                userDiscount = 5;
            }
            if (roles.Contains("S_User"))
            {
                userDiscount = 10;
            }
            foreach (CartItem ci in cart.CartItems)
            {
                currTotal += ci.Product.Price* (1- (ci.Product.Discount + userDiscount)/100);
            }
            cart.SumPayment = currTotal;
            ViewData["Total"] = string.Format("{0:n0}", currTotal);
            return View(cart);
        }

        public IActionResult ConfirmOrder()
        {
            UserID = _userManager.GetUserId(User);
            ApplicationUser user = _context.Users.Single(i => i.Id == UserID);
            double userDiscount = 0;
            string roles = _userManager.GetRolesAsync(user).ToString();
            if (roles.Contains("A_User"))
            {
                userDiscount = 5;
            }
            if (roles.Contains("S_User"))
            {
                userDiscount = 10;
            }
            ViewData["userDiscount"] = userDiscount; 
            var cart = _context.Carts
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
                if (history.HistoryItems == null)
                {
                    history.HistoryItems = new List<HistoryItems>();
                }
                history.HistoryItems.Add(historyStuff);
                historyStuff.CreateDate = DateTime.Now;
                c.Product.Sell++;
            }
            user.Expense += cart.SumPayment;

            cart.CartItems.Clear();
            cart.SumPayment = 0;
            
            _context.Update(history);
            _context.Update(cart);
            _context.Update(user);
            _context.SaveChangesAsync();
            return View(RedirectToAction(nameof(HistoryPage)));
        }
 
        public IActionResult HistoryPage()
        {
            UserID = _userManager.GetUserId(User);

            var history = _context.History
                .Include(d => d.HistoryItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.UserId == UserID);
            if (history == null || history.HistoryItems == null)
            {
                return View();
            }
            double? currTotal = 0;
            foreach (HistoryItems ci in history.HistoryItems)
            {
                currTotal += ci.Product.FinalPrice;
            }
            ViewData["Total"] = string.Format("{0:n0}", currTotal);
            return View(history.HistoryItems.ToList());
        }

    }
}