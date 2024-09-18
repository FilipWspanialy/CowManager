using CowManager.Models.Models;
using CowManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CowManagerApp.Areas.Admin.Controllers
{[Area("Admin")]
    [Authorize(Roles = "Admin")]
    
    public class DiseaseController : Controller
    {
        private readonly CowManagerContext _context;
        public DiseaseController(CowManagerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (_context.Diseases == null)
            {
                return Problem("Entity set 'ApiContext.Movie'  is null.");
            }

            var dises = _context.Diseases;

            return View(await dises.ToListAsync());
        }
        public async Task<IActionResult> DiseaseDetails(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && !referer.Contains("Edit"))
            {
                HttpContext.Session.SetString("PreviousUrl", referer);
                ViewBag.PreviousUrl = referer;

            }
            else
            {
                ViewBag.PreviousUrl = HttpContext.Session.GetString("PreviousUrl");
            }

            if (id == null)
            {
                return NotFound();
            }

            var dises = await _context.Diseases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dises == null)
            {
                return NotFound();
            }

            return View(dises);
        }
        public async Task<IActionResult> DiseaseCreate()
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiseaseCreate([Bind("Id, Name, Comment")] Disease dis, string previousUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dis);
                await _context.SaveChangesAsync();
                return Redirect(previousUrl);
            }
            return View(dis);
        }

        public async Task<IActionResult> DiseaseEdit(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

            if (id == null)
            {
                return NotFound();
            }

            var dis = await _context.Diseases.FindAsync(id);
            if (dis == null)
            {
                return NotFound();
            }

            return View(dis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiseaseEdit(int id, [Bind("Id, Name, Comment")] Disease dis, string previousUrl)
        {
            if (id != dis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dis);
                    await _context.SaveChangesAsync();
                    return Redirect(previousUrl);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiseaseExists(dis.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(dis);
        }

        public async Task<IActionResult> DiseaseDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dis = await _context.Diseases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dis == null)
            {
                return NotFound();
            }

            return View(dis);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiseaseDeleteConfirmed(int id)
        {
            var dis = await _context.Diseases
                .FirstOrDefaultAsync(h => h.Id == id);

            var diag = await _context.Diagnoses
                .Where(h => h.Iddisease == id)
                .ToListAsync();

            if (dis != null)
            {
                if (diag != null && diag.Any())
                {
                    var diagIds = diag.Select(d => d.Id).ToList();

                    var treatfordiag = await _context.Treatments
                        .Where(t => t.Iddiagnosis.HasValue && diagIds.Contains(t.Iddiagnosis.Value))
                        .ToListAsync();

                    _context.Diagnoses.RemoveRange(diag);
                    _context.Treatments.RemoveRange(treatfordiag);
                }

                _context.Diseases.Remove(dis);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        private bool DiseaseExists(int id)
        {
            return _context.Diseases.Any(e => e.Id == id);
        }
    }
}

