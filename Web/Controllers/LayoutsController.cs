using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LayoutsController : Controller
    {
        [ActionName("_horizontal")]
        public IActionResult _horizontal()
        {
            return View();
        }
    }
}
