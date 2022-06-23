using Computer_Store.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Computer_Store.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index(int userId)
        //{
        //    if (userId == null)
        //    {
        //        return NotFound();
        //    }
        //    var allCartItems = await _context.Carts
        //        .Include(c => c.CItems)
        //        .ThenInclude(p => p.Product)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(i => i.UserId  == userId);
        //    if (allCartItems == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(allCartItems);
        //}


 
    }
}
