using Computer_Store.Data;
using Computer_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Computer_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
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
    }
}