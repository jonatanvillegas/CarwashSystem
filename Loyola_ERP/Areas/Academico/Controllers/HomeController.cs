using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Academico.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
