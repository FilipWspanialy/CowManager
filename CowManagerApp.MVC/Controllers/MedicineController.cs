using Microsoft.AspNetCore.Mvc;

namespace CowManagerApp.MVC.Controllers
{
    public class MedicineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
