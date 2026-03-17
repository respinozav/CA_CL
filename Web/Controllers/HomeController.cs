using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.Data;
using Web.Filters;
using Web.Models;


namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            UsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        [MenuAuthorize]
        public IActionResult MisDatos()
        {
            var usuarioIdString = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(usuarioIdString))
                return RedirectToAction("Login", "Authentication");

            var usuarioId = Guid.Parse(usuarioIdString);

            var usuario = _usuarioRepository.ObtenerUsuario(usuarioId);

            return View(usuario);
        }

        [MenuAuthorize]
        public IActionResult Pago()
        {
            return View();
        }
        [MenuAuthorize]
        public IActionResult CambiodeClave()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

