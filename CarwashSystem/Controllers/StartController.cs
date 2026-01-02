using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Wellcome", new { Area = "Home" });
        }
    }
}
