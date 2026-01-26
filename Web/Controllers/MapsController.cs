using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MapsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
