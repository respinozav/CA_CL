using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
