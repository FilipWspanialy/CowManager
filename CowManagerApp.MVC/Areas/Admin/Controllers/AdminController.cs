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

        // Skonsolidowany konstruktor z wstrzykiwaniem dwóch zależności
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
        // Wyświetlanie szczegółów użytkownika
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch all cows
            var cows = await _context.Cows.ToListAsync();

            // Filter cows that belong to the current user
            var userCows = cows.Where(c => c.UserId == id).ToList();

            // Fetch all diagnoses
            var diagnoses = await _context.Diagnoses.ToListAsync();

            // Filter diagnoses based on cows belonging to the current user
            var userDiagnoses = diagnoses.Where(d => userCows.Select(c => c.Id).Contains(d.Idcow)).ToList();

            // Create the view model
            var model = new UserDetailsViewModel
            {
                User = user,
                Cows = userCows,
                Diagnoses = userDiagnoses,
                CurrentUserId = id
            };

            return View(model);
        }
    }

    
}

