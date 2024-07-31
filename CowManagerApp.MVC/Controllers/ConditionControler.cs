using Microsoft.AspNetCore.Mvc;

namespace CowManagerApp.MVC.Controllers
{
    public class ConditionControler : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
