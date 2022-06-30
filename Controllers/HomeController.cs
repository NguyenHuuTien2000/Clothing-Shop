using Computer_Store.Data;
using Computer_Store.Models;
using Computer_Store.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
                ViewData["Type"] = type;
                if (Enum.TryParse(type, out ComputerType reqType))
                {
                    allComputers = allComputers.Where(c => c.Type == reqType).ToList();
                }
            }

            if (brand != null)
            {
                ViewData["Brand"] = brand;
                if (Enum.TryParse(brand, out Brand reqBrand))
                {
                    if (reqBrand == Brand.AMD)
                    {
                        allComputers = allComputers.Where(c => c.Spec.CPUDetail.Contains("AMD")).ToList();
                    } 
                    else if (reqBrand == Brand.Intel)
                    {
                        allComputers = allComputers.Where(c => c.Spec.CPUDetail.Contains("Intel")).ToList();
                    } 
                    else
                    {
                        allComputers = allComputers.Where(c => c.Brand == reqBrand).ToList();
                    }
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

        public IActionResult CartPage(string? type, string? brand, string? category, string searchString, string sortOrder)
        {
            ViewData["PriceSort"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_asc";
            ViewData["NameSort"] = searchString;
            UserID = _userManager.GetUserId(User);

            var cart = _context.Carts
                .Include(d => d.CartItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.UserId == UserID);
            
            if (cart == null || cart.CartItems == null)
            {
                return View();
            }

            var allItems = cart.CartItems.ToList();

            ViewData["CurrentType"] = "All";
            if (type != null)
            {
                ViewData["Type"] = type;
                ViewData["CurrentType"] = type;
                if (type.Equals("Computer"))
                {
                    allItems = allItems.Where(c => c.Product.GetType() == typeof(Computer)).ToList();
                }
                else
                {
                    allItems = allItems.Where(c => c.Product.GetType() == typeof(Part)).ToList();
                }
            }

            if (category != null && type != null)
            {
                var newList = new List<CartItem>();
                ViewData["Category"] = category;
                if (type.Equals("Computer"))
                {
                    foreach (var item in allItems)
                    {
                        if (item.Product.GetType() == typeof(Computer))
                        {
                            Computer computer = (Computer)item.Product;
                            if (computer.Category.ToString().Equals(category))
                            {
                                newList.Add(item);
                            }
                        }
                    }
                }

                if (type.Equals("Part"))
                {
                    foreach (var item in allItems)
                    {
                        if (item.Product.GetType() == typeof(Part))
                        {
                            Part computer = (Part)item.Product;
                            if (computer.Category.ToString().Equals(category))
                            {
                                newList.Add(item);
                            }
                        }
                    }
                }

                if (newList.Count > 0)
                {
                    allItems = newList;
                }
            }

            if (brand != null)
            {
                ViewData["Brand"] = brand;
                if (Enum.TryParse(brand, out Brand reqBrand))
                {
                    allItems = allItems.Where(c => c.Product.Brand == reqBrand).ToList();
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allItems = allItems.Where(s => s.Product.Name.ToLower().Contains(searchString)).ToList();
            }

            ViewData["CurrentSort"] = "Ascending";

            switch (sortOrder)
            {
                case "price_desc":
                    allItems = allItems.OrderByDescending(p => p.Product.FinalPrice).ToList();
                    ViewData["CurrentSort"] = "Descending";
                    break;
                case "price_asc":
                    allItems = allItems.OrderBy(p => p.Product.FinalPrice).ToList();
                    break;
                default:
                    allItems = allItems.OrderBy(p => p.Product.FinalPrice).ToList();
                    break;
            }

            double? currTotal = 0; 
            foreach (CartItem ci in cart.CartItems)
            {
                currTotal += ci.Product.FinalPrice * ci.Quantity;
            }
            ViewData["Total"] = string.Format("{0:n0}", currTotal);
            ViewData["GotItem"] = cart.CartItems.Count > 0;
            return View(allItems);
        }

        public IActionResult AddToCart(int? pid, int quantity)
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
                    ci.Quantity += quantity;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(CartPage));
                }
            }
            var cartItem = new CartItem();
            cartItem.CartID = cart.Id;
            cartItem.ProductID = product.Id;
            cartItem.Product = product;
            cartItem.MyCart = cart;
            cartItem.Quantity = quantity;
            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }
            cart.CartItems.Add(cartItem);
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(CartPage));
        }

        public IActionResult UpdateQuantity(CartUpdate[] updates)
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

            foreach (var item in updates)
            {
                CartItem? toUpdate = itemList.FirstOrDefault(c => c.Id == item.Id);
                if (toUpdate != null)
                {
                    toUpdate.Quantity = item.Quantity;
                }
            }
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

        public async Task<IActionResult> OrderPage()
        {
            UserID = _userManager.GetUserId(User);
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(b => b.Product)
                .Single(x => x.UserId == UserID);

            double? currTotal = 0;
            ApplicationUser user = _context.Users.Single(i => i.Id == UserID);
            double userDiscount = 0;
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);

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
                ci.CustomDiscount = userDiscount;
                currTotal += ci.Price;
            }
            cart.SumPayment = currTotal;
            ViewData["Total"] = string.Format("{0:n0}", currTotal);
            return View(cart);
        }

        public async Task<IActionResult> ConfirmOrder(string? altDelivery, string? payment)
        {
            UserID = _userManager.GetUserId(User);

            ApplicationUser user = _userManager.Users.Single(i => i.Id == UserID);
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);

            var todateReport = _context.DailyReports.FirstOrDefault(d => DateTime.Compare(d.Date, DateTime.Today) == 0);
            
            if (todateReport == null)
            {
                todateReport = new DailyReport
                {
                    Date = DateTime.Today,
                    TotalRevenue = 0,
                    TotalUnit = 0,
                    DateString = DateTime.Today.ToString("dd/M")
                };
                _context.Add(todateReport);
            }

            if (altDelivery == null)
            {
                altDelivery = user.Address;
            }

            if (payment == null)
            {
                payment = "COD";
            } 
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(b => b.Product)
                .Single(x => x.UserId == UserID);
            
            cart.SumPayment = cart.CartItems.Sum(c => c.Price);

            var order = new Order
            {
                UserID = user.Id,
                Status = "Pending",
                CreatedDate = DateTime.Today,
                Total = cart.SumPayment,
                PaymentMethod = payment,
                DeliveryAddress = altDelivery
            };
            order.OrderItems = new List<OrderItem>();

            foreach (CartItem c in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderID = order.Id,
                    ProductID = c.ProductID,
                    Product = c.Product,
                    Quantity = c.Quantity,
                    Price = c.Price
                };
                order.OrderItems.Add(orderItem);
                order.ItemNum += orderItem.Quantity;
                c.Product.Sell++;
            }
            _context.Add(order);

            todateReport.TotalRevenue += cart.SumPayment;
            todateReport.TotalUnit += cart.CartItems.Sum(c => c.Quantity);
            user.Expense += cart.SumPayment;

            string promoted = null;
            if (user.Expense >= 100_000_000 && !roles.Contains("A_User"))
            {
                promoted = "A Rank";
                roles.Add("A_User");
                await _userManager.AddToRolesAsync(user, roles);
            }

            if (user.Expense >= 250_000_000 && !roles.Contains("S_User"))
            {
                promoted = "S Rank";
                roles.Add("S_User");
                await _userManager.AddToRolesAsync(user, roles);
            }

            cart.CartItems.Clear();
            cart.SumPayment = 0;
            var sortedList = _context.Products.ToList();
            sortedList.Sort((a, b) => b.Sell - a.Sell);
            todateReport.MostBoughtCategory = sortedList[0].Name;
            todateReport.SecondBoughtCategory = sortedList[1].Name;

            _context.Update(cart);
            _context.Update(user);
            _context.Update(todateReport);
            await _context.SaveChangesAsync();
         
            return RedirectToAction(nameof(OrderList), promoted);
        }

        public IActionResult OrderList(string? promoted, string? quantity, string? day, string? sortOrder)
        {
            

            UserID = _userManager.GetUserId(User);

            var allOrders = _context.Orders
                .Where(c => c.UserID == UserID)
                .Where(c => c.Status == "Pending")
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .ToList();

            if (allOrders == null)
            {
                return View();
            }

            ViewData["TotalSort"] = "Ascending";
            switch (sortOrder)
            {
                case "price_desc":
                    allOrders = allOrders.OrderByDescending(p => p.Total).ToList();
                    ViewData["TotalSort"] = "Descending";
                    break;
                case "price_asc":
                    allOrders = allOrders.OrderBy(p => p.Total).ToList();
                    break;
                default:
                    break;
            }

            ViewData["QuantitySort"] = "Ascending";
            switch (quantity)
            {
                case "q_desc":
                    allOrders = allOrders.OrderByDescending(p => p.ItemNum).ToList();
                    ViewData["QuantitySort"] = "Descending";
                    break;
                case "q_asc":
                    allOrders = allOrders.OrderBy(p => p.ItemNum).ToList();
                    break;
                default:
                    break;
            }

            ViewData["DateSort"] = "Ascending";
            switch (day)
            {
                case "date_desc":
                    allOrders = allOrders.OrderByDescending(p => p.CreatedDate).ToList();
                    ViewData["DateSort"] = "Descending";
                    break;
                case "date_asc":
                    allOrders = allOrders.OrderBy(p => p.CreatedDate).ToList();
                    break;
                default:
                    break;
            }

            ViewData["Promoted"] = promoted;
            ViewData["GotItem"] = true;
            return View(allOrders);
        }

        public IActionResult OrderDetail(int oid)
        {
            UserID = _userManager.GetUserId(User);

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .AsNoTracking()
                .FirstOrDefault(o => o.Id == oid);
            if (order == null)
            {
                return NotFound();
            }
            return View(order.OrderItems.ToList());
        }

        public ActionResult SaleReport()
        {
            UserID = _userManager.GetUserId(User);
            var dailyReport = _context.DailyReports.ToList();
            dailyReport.Sort((a, b) => DateTime.Compare(b.Date, a.Date));
            return View(dailyReport);
        }

        public IActionResult HistoryPage(string? quantity, string? day, string? sortOrder)
        {
            UserID = _userManager.GetUserId(User);
            var history = _context.History
                .Include(h => h.Orders)
                .FirstOrDefault(h => h.UserId == UserID);
            if (history == null)
            {
                ViewData["GotItem"] = false;
                return View();
            }
            var allOrders = history.Orders.Where(o => o.Status == "Completed").ToList();

            if (!allOrders.Any())
            {
                ViewData["GotItem"] = false;
                return View();
            }

            ViewData["TotalSort"] = "Ascending";
            switch (sortOrder)
            {
                case "price_desc":
                    allOrders = allOrders.OrderByDescending(p => p.Total).ToList();
                    ViewData["TotalSort"] = "Descending";
                    break;
                case "price_asc":
                    allOrders = allOrders.OrderBy(p => p.Total).ToList();
                    break;
                default:
                    break;
            }

            ViewData["QuantitySort"] = "Ascending";
            switch (quantity)
            {
                case "q_desc":
                    allOrders = allOrders.OrderByDescending(p => p.ItemNum).ToList();
                    ViewData["QuantitySort"] = "Descending";
                    break;
                case "q_asc":
                    allOrders = allOrders.OrderBy(p => p.ItemNum).ToList();
                    break;
                default:
                    break;
            }

            ViewData["DateSort"] = "Ascending";
            switch (day)
            {
                case "date_desc":
                    allOrders = allOrders.OrderByDescending(p => p.CreatedDate).ToList();
                    ViewData["DateSort"] = "Descending";
                    break;
                case "date_asc":
                    allOrders = allOrders.OrderBy(p => p.CreatedDate).ToList();
                    break;
                default:
                    break;
            }

            ViewData["GotItem"] = true;
            return View(allOrders);
        }
    }
}