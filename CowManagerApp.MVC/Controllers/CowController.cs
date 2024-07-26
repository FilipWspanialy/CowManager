using CowManagerApp.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CowManager.Controllers
{
    public class CowController : Controller
    {
        private readonly CowManagerContext _context;
        public CowController(CowManagerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (_context.Cows == null)
            {
                return Problem("Entity set 'ApiContext.Movie'  is null.");
            }

            var cows = _context.Cows;

            return View(await cows.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cows = await _context.Cows
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cows == null)
            {
                return NotFound();
            }

            return View(cows);
        }

        public async Task<IActionResult> Create()
        {
            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Id");

            // Diagnostyka: sprawdź, czy ViewBag.Herds zawiera dane
            if (herds == null || !herds.Any())
            {
                ViewBag.HerdsError = "No herds available.";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Idherd,Comment")] Cow cows)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cows);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Id", cows.Idherd);

            return View(cows);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Id");
            if (id == null)
            {
                return NotFound();
            }

            var cows = await _context.Cows.FindAsync(id);
            if (cows == null)
            {
                return NotFound();
            }
            return View(cows);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Idherd,Comment")] Cow cows)
        {
            if (id != cows.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cows);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CowExists(cows.Id))
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
            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Id", cows.Idherd);
            return View(cows);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cows = await _context.Cows
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cows == null)
            {
                return NotFound();
            }

            return View(cows);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cows = await _context.Cows.FindAsync(id);
            if (cows != null)
            {
                _context.Cows.Remove(cows);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CowExists(int id)
        {
            return _context.Cows.Any(e => e.Id == id);
        }
        
    }
}
