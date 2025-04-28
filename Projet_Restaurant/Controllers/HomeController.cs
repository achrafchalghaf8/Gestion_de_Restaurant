using Microsoft.AspNetCore.Mvc;

namespace Projet_Restaurant.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
