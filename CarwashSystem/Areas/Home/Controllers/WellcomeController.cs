using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Home.Controllers
{
    [Area("Home")]
    public class WellcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
