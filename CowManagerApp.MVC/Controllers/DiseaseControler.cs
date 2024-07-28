using CowManagerApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CowManagerApp.MVC.Controllers
{
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
        public async Task<IActionResult> DIseaseDetails(int? id)
        {
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
    }
}
