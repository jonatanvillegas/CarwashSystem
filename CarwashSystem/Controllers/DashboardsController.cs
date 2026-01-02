using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class DashboardsController : Controller
    {
        public IActionResult Index1() => View();
        public IActionResult Index2() => View();
        public IActionResult Index3() => View();
    }
}
