using CowManager.Models;
using CowManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CowManager.Models.Models;

namespace CowManagerApp.Areas.Admin.Controllers
{[Area("Admin")]
    [Authorize(Roles = "Admin")]
    
    public class MedicineController : Controller
    {
        private readonly CowManagerContext _context;
        public MedicineController(CowManagerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (_context.Medicines == null)
            {
                return Problem("Entity set 'ApiContext.Movie'  is null.");
            }

            var meds = _context.Medicines;

            return View(await meds.ToListAsync());
        }
        public async Task<IActionResult> MedicineDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meds == null)
            {
                return NotFound();
            }

            return View(meds);
        }
        public async Task<IActionResult> MedicineCreate()
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicineCreate([Bind("Id, Name, Comment")] Medicine med, string previousUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(med);
                await _context.SaveChangesAsync();
                return Redirect(previousUrl);
            }
            return View(med);
        }

        public async Task<IActionResult> MedicineEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines.FindAsync(id);
            if (meds == null)
            {
                return NotFound();
            }

            return View(meds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicineEdit(int id, [Bind("Id, Name, Comment")] Medicine med)
        {
            if (id != med.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(med);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(med.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(med);
        }

        public async Task<IActionResult> MedicineDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meds == null)
            {
                return NotFound();
            }

            return View(meds);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicineDeleteConfirmed(int id)
        {
            var meds = await _context.Medicines
               .FirstOrDefaultAsync(h => h.Id == id);
            var treat = _context.Treatments
                .Where(h => h.Idmedicine == id);

            if (meds != null)
            {
                if (treat != null)
                {
                    _context.Treatments.RemoveRange(treat);
                }
                _context.Medicines.Remove(meds);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
