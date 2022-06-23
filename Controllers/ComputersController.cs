using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Computer_Store.Data;
using Computer_Store.Models;

namespace Computer_Store.Controllers
{
    public class ComputersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Computers
        public async Task<IActionResult> Index()
        {
            var allComputers = _context.Computers.Include(c => c.Spec);
            return View(await allComputers.ToListAsync());
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Spec)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        // GET: Computers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Computers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,Type,Id,Name,Price,Discount,Image,Brand")] Computer computer, IFormFile Image, ComputerSpec spec)
        {
            if (Image.Length > 0 && spec != null)
            {
                string imageFolder = Path.Combine("images", "computers", computer.Brand + "");
                string storedImage = Path.Combine("wwwroot", imageFolder);
                if (!Directory.Exists(storedImage))
                {
                    Directory.CreateDirectory(storedImage);
                }

                storedImage = Path.Combine(storedImage, Image.FileName);
                using (Stream fileStream = new FileStream(storedImage, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                computer.Image = Path.Combine(imageFolder, Image.FileName);
                computer.SpecID = spec.Id;
                computer.Spec = spec;
            }

            if (ModelState.IsValid)
            {
                _context.Add(computer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        // GET: Computers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Spec)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (computer == null)
            {
                return NotFound();
            }
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        // POST: Computers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category,Type,SpecID,Id,Name,Price,Discount,Brand")] Computer computer, IFormFile? Image, ComputerSpec spec)
        {
            if (id != computer.Id)
            {
                return NotFound();
            }

            var oldPath = _context.Computers.AsNoTracking().FirstOrDefault(c => c.Id == id).Image;
            if (Image != null)
            {
                if (oldPath != null)
                {
                    FileInfo fileInfo = new(Path.Combine("wwwroot", oldPath));
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }

                string imageFolder = Path.Combine("images", "computers", computer.Brand + "");
                string storedImage = Path.Combine("wwwroot", imageFolder);
                if (!Directory.Exists(storedImage))
                {
                    Directory.CreateDirectory(storedImage);
                }
                storedImage = Path.Combine(storedImage, Image.FileName);

                using (Stream fileStream = new FileStream(storedImage, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                computer.Image = Path.Combine(imageFolder, Image.FileName);
            }
            else
            {
                computer.Image = oldPath;
            }


            if (spec != null)
            {
                computer.SpecID = spec.Id;
                computer.Spec = spec;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerExists(computer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        // GET: Computers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Computers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers
                .Include(c => c.Spec)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }
            ViewData["Image"] = computer.Image;
            ViewData["Spec"] = computer.Spec;
            return View(computer);
        }

        // POST: Computers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Computers == null)
            {
                return Problem("No computer found in DB");
            }
            var computer = await _context.Computers
                .Include(c => c.Spec)
                .SingleOrDefaultAsync(c => c.Id == id);
            if (computer != null)
            {
                _context.Computers.Remove(computer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerExists(int id)
        {
          return (_context.Computers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
