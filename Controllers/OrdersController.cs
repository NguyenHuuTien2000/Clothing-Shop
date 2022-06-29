using Computer_Store.Data;
using Computer_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var allOrders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(p => p.Product)
                .ToList();
            ViewData["GotItem"] = allOrders != null && allOrders.Any();
            return View(allOrders);
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

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
