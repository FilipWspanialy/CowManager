using CowManager.Models.Models;
using CowManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CowManagerApp.Areas.Admin.Controllers
{ [Area("Admin")]
    [Authorize(Roles = "Admin")]
   

    public class HerdController : Controller
    {
        private readonly CowManagerContext _context;
        private UserManager<IdentityUser> _userManager;

        public HerdController(CowManagerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.Herds == null)
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
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

            var herd = await _context.Herds
                .Include(s => s.Cows)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (herd == null)
            {
                return NotFound();
            }

            return View(herd);
        }
        public async Task<IActionResult> HerdCreate()
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HerdCreate([Bind("Id,Comment,UserId")] Herd herds, string previousUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(herds);
                await _context.SaveChangesAsync();
                return Redirect(previousUrl);
            }
            return View(herds);
        }
        public async Task<IActionResult> HerdDelete(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

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
        public async Task<IActionResult> HerdDeleteConfirmed(int id, string previousUrl)
        {
            var herd = await _context.Herds
               .Include(h => h.Cows)
               .FirstOrDefaultAsync(h => h.Id == id);

            if (herd != null)
            {
                if (herd.Cows != null)
                {
                    _context.Cows.RemoveRange(herd.Cows);
                }
                _context.Herds.Remove(herd);
                await _context.SaveChangesAsync();
            }

            return Redirect(previousUrl);
        }
        public async Task<IActionResult> HerdEdit(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
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
        public async Task<IActionResult> HerdEdit(int id, [Bind("Id,Comment")] Herd herd, string previousUrl)
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
                    return Redirect(previousUrl);
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
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

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
            return View(new Cow { Idherd = Idh, UserId = herd.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCow([Bind("Name,Idherd,Comment,UserId")] Cow cow, string previousUrl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cow);
                await _context.SaveChangesAsync();
                return Redirect(previousUrl);
            }

            var herd = await _context.Herds.FindAsync(cow.Idherd);
            ViewBag.HerdId = cow.Idherd;
            return View(cow);
        }

    }

}

