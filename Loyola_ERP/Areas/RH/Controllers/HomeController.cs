using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.RH.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
