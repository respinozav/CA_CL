using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers
{

    [MenuAuthorize] // usa tu filtro de seguridad
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
