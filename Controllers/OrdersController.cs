using Computer_Store.Data;
using Computer_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Computer_Store.Controllers
{
    [Authorize(Roles ="Moderator,Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        
        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? sortOrder, string? quantity, string? day, int? pageNumber)
        {
            var allOrders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product);

            
            var orders = from o in _context.Orders select o;
            ViewData["TotalSort"] = "Ascending";
            switch (sortOrder)
            {
                case "price_desc":
                    orders = orders.OrderByDescending(p => p.Total);
                    ViewData["TotalSort"] = "Descending";
                    break;
                case "price_asc":
                    orders = orders.OrderBy(p => p.Total);
                    break;
                default:
                    break;
            }

            ViewData["QuantitySort"] = "Ascending";
            switch (quantity)
            {
                case "q_desc":
                    orders = orders.OrderByDescending(p => p.ItemNum);
                    ViewData["QuantitySort"] = "Descending";
                    break;
                case "q_asc":
                    orders = orders.OrderBy(p => p.ItemNum);
                    break;
                default:
                    break;
            }

            ViewData["DateSort"] = "Ascending";
            switch (day)
            {
                case "date_desc":
                    orders = orders.OrderByDescending(p => p.CreatedDate);
                    ViewData["DateSort"] = "Descending";
                    break;
                case "date_asc":
                    orders = orders.OrderBy(p => p.CreatedDate);
                    break;
                default:
                    break;
            }

            int pageSize = 10;
            ViewData["GotItem"] = allOrders != null && allOrders.Any();
            return View(await PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult ApproveOrder(int oid)
        {
            var order = _context.Orders
                .Where(c => c.Status == "Pending")
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.Id == oid);

            if (order == null)
            {
                return NotFound();
            }
            order.Status = "Completed";
            var history = _context.History
                .Include(h => h.Orders)
                .Single(h => h.UserId == order.UserID);
            history.Orders.Add(order);
            _context.Update(history);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
