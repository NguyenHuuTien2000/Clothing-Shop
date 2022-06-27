using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Computer_Store.Data;
using Computer_Store.Models;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Computer_Store.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class PartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Parts
        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewData["PriceSort"] = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_asc";
            ViewData["NameSort"] = searchString;

            var allParts = _context.Parts.AsNoTracking().ToList();

            //if (category != null)
            //{
            //    if (Enum.TryParse(category, out PartCategory reqCategory))
            //    {
            //        allParts = allParts.Where(c => c.Category == reqCategory).ToList();
            //    }
            //}

            //if (brand != null)
            //{
            //    if (Enum.TryParse(brand, out Brand reqBrand))
            //    {
            //        allParts = allParts.Where(c => c.Brand == reqBrand).ToList();
            //    }
            //}

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

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
}
            ViewData["PartImage"] = part.Image;
            return View(part);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,SummarySpec,Id,Name,Price,Discount,Image,Brand")] Part part, IFormFile Image)
        {
            if (Image.Length > 0)
            {
                string imageFolder = Path.Combine("images", "parts", part.Brand + "");
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

                part.Image = Path.Combine(imageFolder, Image.FileName);
            }

            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PartImage"] = part.Image;
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            ViewData["PartImage"] = part.Image;
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category,SummarySpec,Id,Name,Price,Discount,Brand")] Part part, IFormFile? Image)
        {
            if (id != part.Id)
            {
                return NotFound();
            }

            var oldPath = _context.Parts.AsNoTracking().FirstOrDefault(c => c.Id == id).Image;
            if (Image != null)
            {
                if (oldPath != null)
                {
                    FileInfo fileInfo = new FileInfo(Path.Combine("wwwroot", oldPath));
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }

                string imageFolder = Path.Combine("images", "parts", part.Brand + "");
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

                part.Image = Path.Combine(imageFolder, Image.FileName);
            }
            else
            {
                part.Image = oldPath;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id))
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
            ViewData["PartImage"] = part.Image;
            return View(part);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }
            ViewData["PartImage"] = part.Image;
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Parts == null)
            {
                return Problem("No part found in DB");
            }
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartExists(int id)
        {
          return (_context.Parts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
