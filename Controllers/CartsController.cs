using Computer_Store.Data;
using Microsoft.AspNetCore.Mvc;

namespace Computer_Store.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    var allCartItems = _context.CartItems.;
        //    return View(await allComputers.ToListAsync());
        //}
    }
}
