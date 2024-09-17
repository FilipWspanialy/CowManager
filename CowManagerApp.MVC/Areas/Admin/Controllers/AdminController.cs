using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using CowManager.Models.Models;
using CowManager.Data;
using Microsoft.EntityFrameworkCore;
namespace CowManagerApp.Areas.Admin.Controllers
{




    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CowManagerContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(CowManagerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> UsersList()
        {
            // Pobierz wszystkich użytkowników z rolą "Customer"
            var usersInRole = await _userManager.GetUsersInRoleAsync("Costumer");

            return View(usersInRole);
        }
        public async Task<IActionResult> UserDetails(string id, string searchString)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userHerds = await _context.Herds
                .Where(h => h.UserId == id) 
                .ToListAsync();

            var userCows = await _context.Cows
                .Where(c => c.UserId == id)
                .ToListAsync();
            
            foreach (var cow in userCows)
            {
                if (cow.DeathDate.HasValue && !cow.IsInactive)
                {
                    cow.IsInactive = true;
                    _context.SaveChanges();
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                userCows = userCows.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var userDiagnoses = await _context.Diagnoses
                .Where(d => userCows.Select(c => c.Id).Contains(d.Idcow))
                .ToListAsync();

            var model = new UserDetailsViewModel
            {
                User = user,
                Herd = userHerds, 
                Cows = userCows,
                Diagnoses = userDiagnoses,
                CurrentUserId = id
            };
            return View(model);
        }

       


    }
}

