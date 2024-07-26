using CowManagerApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CowManagerApp.MVC.Controllers
{
    
    public class HerdController : Controller
    {
        private readonly CowManagerContext _context;

        public HerdController(CowManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(_context.Herds == null)
            {
                return Problem("Entity set 'ApiContext.Movie'  is null.");
            }

            var herds = await _context.Herds
                .Include(s => s.Cows)
                .ToListAsync();

            return View(herds);
        }

        public async Task<IActionResult> HerdDetails(int id)
        {
            var herd = await _context.Herds
                .Include(s => s.Cows)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (herd == null)
            {
                return NotFound();
            }

            return View(herd);
        }
        public IActionResult HerdCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HerdCreate([Bind("Id,Comment")] Herd herds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(herds);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(herds);
        }
        public async Task<IActionResult> HerdDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var herds = await _context.Herds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (herds == null)
            {
                return NotFound();
            }

            return View(herds);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HerdDeleteConfirmed(int id)
        {
            var herds = await _context.Herds.FindAsync(id);
            if (herds != null)
            {
                _context.Herds.Remove(herds);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> HerdEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var herd = await _context.Herds.FindAsync(id);
            if (herd == null)
            {
                return NotFound();
            }

            return View(herd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HerdEdit(int id, [Bind("Id,Comment")] Herd herd)
        {
            if (id != herd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(herd);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HerdExists(herd.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
            }
            return View(herd);
        }

        private bool HerdExists(int id)
        {
            return _context.Herds.Any(e => e.Id == id);
        }
        public async Task<IActionResult> AddCow(int? Idh)
        {
            if (Idh == null)
            {
                return NotFound();
            }

            var herd = await _context.Herds.FindAsync(Idh);
            if (herd == null)
            {
                return NotFound();
            }

            ViewBag.HerdId = Idh;
            return View(new Cow { Idherd = Idh });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCow([Bind("Name,Idherd,Comment")] Cow cow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cow);
                await _context.SaveChangesAsync();
                return RedirectToAction("HerdDetails", "Herd", new { id = cow.Idherd });
            }

            var herd = await _context.Herds.FindAsync(cow.Idherd);
            ViewBag.HerdId = cow.Idherd;
            return View(cow);
        }

    }

}

