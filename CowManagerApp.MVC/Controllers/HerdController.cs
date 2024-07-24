using CowManagerApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

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

            var herds = _context.Herds;

            return View(await herds.ToListAsync());
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
       
    }
}
