﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Computer_Store.Data;
using Computer_Store.Models;

namespace Computer_Store.Controllers
{
    public class PartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index()
        {
              return _context.Parts != null ? 
                          View(await _context.Parts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Parts'  is null.");
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
        public async Task<IActionResult> Create([Bind("Category,SummarySpec,Id,Name,Price,Discount,Image,Brand")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category,SummarySpec,Id,Name,Price,Discount,Image,Brand")] Part part)
        {
            if (id != part.Id)
            {
                return NotFound();
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

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Parts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Parts'  is null.");
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
