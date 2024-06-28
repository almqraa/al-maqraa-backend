using Microsoft.AspNetCore.Mvc;

namespace Al_Maqraa.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
