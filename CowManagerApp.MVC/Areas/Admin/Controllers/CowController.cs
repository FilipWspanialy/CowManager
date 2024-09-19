using CowManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using CowManager.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.CodeAnalysis;
namespace CowManagerApp.Areas.Admin.Controllers

{ [Area("Admin")]
    [Authorize(Roles = "Admin")]
   
    public class CowController : Controller
    {
        private readonly CowManagerContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public string url { get; set; }
       

        public CowController(CowManagerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Cows == null)
            {
                return Problem("Entity set 'CowManagerContext.Cows' is null.");
            }

            var user = await _userManager.GetUserAsync(User);
            var userName = user?.UserName; 

            var cows = await _context.Cows.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                cows = cows.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            foreach (var cow in cows)
            {
                if (cow.DeathDate.HasValue && !cow.IsInactive)
                {
                    cow.IsInactive = true;
                    _context.SaveChanges();
                }
            }



            return View(cows);
        }
    

        public async Task<IActionResult> Details(int? id)
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
            var cows = await _context.Cows
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.FindByIdAsync(cows.UserId);
            ViewBag.UserName = user.UserName;
            if (cows == null)
            {
                return NotFound();
            }

            return View(cows);
        }

        public async Task<IActionResult> Create()
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && !referer.Contains("Herd"))
            {
                HttpContext.Session.SetString("PreviousUrl", referer);
                ViewBag.PreviousUrl = referer;

            }
            else
            {
                ViewBag.PreviousUrl = HttpContext.Session.GetString("PreviousUrl");
            }


            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Comment");
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            
            if (herds == null || !herds.Any())
            {
                ViewBag.HerdsError = "No herds available.";
            }
            if (users == null || !users.Any())
            {
                ViewBag.UsersError = "No users available.";
            }
            return View();
        }
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cowid,Name,Idherd,Nameherd,Comment,UserId,UserName, BirthDate, DeathDate")] Cow cows, string previousUrl)
        {

            var user = await _userManager.FindByIdAsync(cows.UserId);
            cows.UserName = user.UserName;
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                
            }
            else if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("", "UserName is null or empty.");
                
            }
            var idherd = await _context.Herds.FindAsync(cows.Idherd);
            cows.Nameherd = idherd.Comment;
            if (ModelState.IsValid)
            {
                bool isCowidExists = await _context.Cows.AnyAsync(c => c.Cowid == cows.Cowid);

                if (isCowidExists)
                {
                    ModelState.AddModelError("Cowid", "Cowid already exists.");
                }
                else
                {
                    _context.Add(cows);
                    await _context.SaveChangesAsync();
                    return Redirect(previousUrl);
                }
            }
            var herds = await _context.Herds.ToListAsync();
            if (herds == null)
            {
                ModelState.AddModelError("", "No herds!");

            }
            else if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("", "There is no herds");

            }
            ViewBag.Herds = new SelectList(herds, "Id", "Comment", cows.Idherd);

            return View(cows);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && !referer.Contains("HerdCreate"))
            {
                HttpContext.Session.SetString("PreviousUrl", referer);
                ViewBag.PreviousUrl = referer;

            }
            else
            {
                ViewBag.PreviousUrl = HttpContext.Session.GetString("PreviousUrl");
            }

            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Comment");
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "UserName");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cowid,Name,Idherd,Comment,UserId,BirthDate,DeathDate")] Cow cows, string previousUrl)
        {
            if (id != cows.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCow = await _context.Cows
                        .Where(c => c.Cowid == cows.Cowid && c.Id != cows.Id)
                        .FirstOrDefaultAsync();

                    if (existingCow != null)
                    {
                        ModelState.AddModelError("Cowid", "Cowid already exists.");
                    }
                    else
                    {
                        _context.Update(cows);
                        await _context.SaveChangesAsync();
                        if (!string.IsNullOrEmpty(previousUrl))
                        {
                            return Redirect(previousUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home"); 
                        }
                    }
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
            }

            var herds = await _context.Herds.ToListAsync();
            ViewBag.Herds = new SelectList(herds, "Id", "Comment", cows.Idherd);
            var user = await _userManager.FindByIdAsync(cows.UserId);
            cows.UserName = user.UserName;
            var idherd = await _context.Herds.FindAsync(cows.Idherd);
            cows.Nameherd = idherd.Comment;
            return View(cows);
        }

        

        public async Task<IActionResult> Delete(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

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
        public async Task<IActionResult> DeleteConfirmed(int id, string previousUrl)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cow = await _context.Cows.FindAsync(id);
                    if (cow != null)
                    {
                        var diagnoses = _context.Diagnoses.Where(d => d.Idcow == id);
                        _context.Diagnoses.RemoveRange(diagnoses);

                        var treatments = _context.Treatments.Where(t => t.Idcow == id);
                        _context.Treatments.RemoveRange(treatments);

                        _context.Cows.Remove(cow);

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return Redirect(previousUrl);
        }

        private bool CowExists(int id)
        {
            return _context.Cows.Any(e => e.Id == id);
        }
        // ********************************************************************************* diagnosis
        public async Task<IActionResult> Diag(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && !referer.Contains("DiagAdd") && !referer.Contains("TreatForDiag") && !referer.Contains("Documentation") && !referer.Contains("DiseaseDetails") && !referer.Contains("DiagRemove") && !referer.Contains("DiagEdit"))
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

            var cow = await _context.Cows
                .Include(c => c.IdherdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cow == null)
            {
                return NotFound();
            }

            var diagnoses = await _context.Diagnoses
                .Where(d => d.Idcow == id && d.DeleteTime == null)
                .Include(d => d.IddiseaseNavigation)
                .ToListAsync();

            var viewModel = new CowDiag
            {
                Cow = cow,
                Diagnoses = diagnoses
            };

            return View(viewModel);
        }
        public async Task<IActionResult> DiagAdd(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows.FindAsync(id);
            if (cow == null)
            {
                return NotFound();
            }

            var diseases = await _context.Diseases.ToListAsync();
            var viewModel = new CowDiagAdd
            {
                CowId = cow.Id,
                CowName = cow.Name,
                Diseases = diseases
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiagAdd(CowDiagAdd model)
        {

            var diagnosis = new Diagnosis
            {
                Idcow = model.CowId,
                Iddisease = model.SelectedDiseaseId,
                NameOfDisease = _context.Diseases.FirstOrDefault(d => d.Id == model.SelectedDiseaseId)?.Name,
                Comment = model.Comment,
                CreateTime = DateTime.Now
            };

            _context.Add(diagnosis);
            await _context.SaveChangesAsync();

            return RedirectToAction("Diag", "Cow", new { id = model.CowId });



        }
        public async Task<IActionResult> DiagRemove(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var diags = await _context.Diagnoses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (diags == null)
            {
                return NotFound();
            }

            return View(diags);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiagRemove(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            diagnosis.DeleteTime = DateTime.Now;

            try
            {
                _context.Update(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction("Diag", "Cow", new { id = diagnosis.Idcow });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiagnosisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return View("Error");
                }
            }
        }
        private bool DiagnosisExists(int id)
        {
            return _context.Diagnoses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> DiagEdit(int? id)
        {

            var diags = await _context.Diagnoses.FindAsync(id);
            if (diags == null)
            {
                return NotFound();
            }
            return View(diags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiagEdit(int id, [Bind("Id, Idcow, Iddisease, NameOfDisease, Comment")] Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return NotFound();
            }


            try
            {
                _context.Update(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction("Diag", "Cow", new { id = diagnosis.Idcow });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CowExists(diagnosis.Id))
                {
                    return NotFound();
                }
                else
                {
                    return View();
                }
            }

        }

        // ********************************************************************************* treatment
        public async Task<IActionResult> Treat(int? id)
        {
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer) && !referer.Contains("TreatAdd") && !referer.Contains("TreatForDiag") && !referer.Contains("Documentation") && !referer.Contains("MedicineDetails") && !referer.Contains("TreatRemove") && !referer.Contains("TreatEdit"))
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

            var cow = await _context.Cows
                .Include(c => c.IdherdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cow == null)
            {
                return NotFound();
            }

            var treats = await _context.Treatments
                .Where(d => d.Idcow == id && d.DeleteTime == null)
                .Include(d => d.IdmedicineNavigation)
                .ToListAsync();

            var viewModel = new CowTreat
            {
                Cow = cow,
                Treatments = treats
            };

            return View(viewModel);
        }
        public async Task<IActionResult> TreatAdd(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows.FindAsync(id);
            if (cow == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines.ToListAsync();
            var viewModel = new CowTreatAdd
            {
                CowId = cow.Id,
                CowName = cow.Name,
                Medicines = meds
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TreatAdd(CowTreatAdd model)
        {

            var treats = new Treatment
            {
                Idcow = model.CowId,
                Idmedicine = model.SelectedMedicinetId,
                NameOfMedicine = _context.Medicines.FirstOrDefault(d => d.Id == model.SelectedMedicinetId)?.Name,
                Comment = model.Comment,
                CreateTime = DateTime.Now
            };

            _context.Add(treats);
            await _context.SaveChangesAsync();

            return RedirectToAction("Treat", "Cow", new { id = model.CowId });



        }
        public async Task<IActionResult> TreatRemove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treats = await _context.Treatments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (treats == null)
            {
                return NotFound();
            }

            return View(treats);
        }

        [HttpPost, ActionName("TreatRemove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TreatRemove(int id, [Bind("DeleteTime")] Treatment treatment)
        {
            var treats = await _context.Treatments.FindAsync(id);
            if (treats == null)
            {
                return NotFound();
            }

            treats.DeleteTime = DateTime.Now;

            try
            {
                _context.Update(treats);
                await _context.SaveChangesAsync();
                return RedirectToAction("treat", "Cow", new { id = treats.Idcow });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiagnosisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> TreatEdit(int? id)
        {

            var treats = await _context.Treatments.FindAsync(id);
            if (treats == null)
            {
                return NotFound();
            }
            return View(treats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TreatEdit(int id, [Bind("Id, Idcow, Idmedicine, NameOfMedicine, Comment")] Treatment treatment)
        {
            if (id != treatment.Id)
            {
                return NotFound();
            }


            try
            {
                _context.Update(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Treat", "Cow", new { id = treatment.Idcow });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CowExists(treatment.Id))
                {
                    return NotFound();
                }
                else
                {
                    return View();
                }
            }

        }
        public async Task<IActionResult> TreatForDiag(int idd, int idk)
        {
            if (idd == null)
            {
                return NotFound();
            }
            if (idk == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows
                .Include(c => c.IdherdNavigation)
                .FirstOrDefaultAsync(m => m.Id == idk);

            if (cow == null)
            {
                return NotFound();
            }

            var treats = await _context.Treatments
                .Where(d => d.Idcow == idk)
                .Where(d => d.Iddiagnosis == idd)
                .Include(d => d.IdmedicineNavigation)
                .ToListAsync();

            var viewModel = new CowTreat
            {
                Cow = cow,
                Treatments = treats
            };
            ViewBag.idd = idd;
            ViewBag.idk = idk;
            ViewBag.Disease = _context.Diagnoses.FirstOrDefault(d => d.Id == idd)?.NameOfDisease;
            return View(viewModel);
        }
        public async Task<IActionResult> TreatForDiagAdd(int idd, int idk)
        {

            if (idd == null)
            {
                return NotFound();
            }
            if (idk == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows.FindAsync(idk);
            if (cow == null)
            {
                return NotFound();
            }

            var diag = await _context.Diagnoses.FindAsync(idd);
            if (diag == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines.ToListAsync();
            var viewModel = new CowTreatAdd
            {
                CowId = cow.Id,
                CowName = cow.Name,
                DiagId = diag.Id,
                Medicines = meds
            };

            return View(viewModel);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TreatForDiagAdd(CowTreatAdd cta)
        {


            var treats = new Treatment
            {
                Idcow = cta.CowId,
                Idmedicine = cta.SelectedMedicinetId,
                Iddiagnosis = cta.DiagId,
                NameOfMedicine = _context.Medicines.FirstOrDefault(d => d.Id == cta.SelectedMedicinetId)?.Name,
                Comment = cta.Comment,
                CreateTime = DateTime.Now
            };



            _context.Add(treats);
            await _context.SaveChangesAsync();
            return RedirectToAction("TreatForDiag", "Cow", new { idd = cta.DiagId, idk = cta.CowId });
        }

        // ********************************************************************************* documentation

        public async Task<IActionResult> Documentation(int id)
        {
            var referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousUrl = referer;

            
            if (id == null)
            {
                return NotFound();
            }

            var cow = await _context.Cows
                .Include(c => c.IdherdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cow == null)
            {
                return NotFound();
            }

            var treats = await _context.Treatments
                .Where(d => d.Idcow == id && d.DeleteTime != null)
                .Include(d => d.IdmedicineNavigation)
                .OrderBy(d => d.DeleteTime)
                .ToListAsync();
            var diags = await _context.Diagnoses
                .Where(d => d.Idcow == id && d.DeleteTime != null)
                .Include(d => d.IddiseaseNavigation)
                .OrderBy(d => d.DeleteTime)
                .ToListAsync();

            var viewModel = new CowDocumentation
            {
                Cow = cow,
                Treatments = treats,
                Diagnosis = diags
            };

            return View(viewModel);
        }
    }
}


        
