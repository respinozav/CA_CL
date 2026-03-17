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
        private readonly CiudadesRepository _ciudadesRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly GenerosRepository _generosRepository;

        public HomeController(
            ILogger<HomeController> logger,
            UsuarioRepository usuarioRepository,
            CiudadesRepository ciudadesRepository,
            GenerosRepository generosRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _ciudadesRepository = ciudadesRepository;
            _generosRepository = generosRepository;
        }

        [MenuAuthorize]
        public IActionResult MisDatos()
        {
            var usuarioIdString = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(usuarioIdString))
                return RedirectToAction("Login", "Authentication");

            var usuarioId = Guid.Parse(usuarioIdString);

            var usuario = _usuarioRepository.ObtenerUsuario(usuarioId);

            // 👇 Crear el ViewModel limpio
            var vm = new MisDatosViewModel
            {
                Usuario = usuario,
                Ciudades = _ciudadesRepository.ListarCiudades(),
                Generos = _generosRepository.ListarGeneros()
            };

            return View(vm);
        }
        [HttpPost]
        [MenuAuthorize]
        public IActionResult GuardarPerfil(MisDatosViewModel model)
        {
            var usuarioIdString = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(usuarioIdString))
                return RedirectToAction("Login", "Authentication");

            var usuarioId = Guid.Parse(usuarioIdString);

            var result = _usuarioRepository.ActualizarUsuario(
                usuarioId,
                model.Usuario.Nombre,
                model.Usuario.CiudadId,
                model.Usuario.GeneroId,
                model.Usuario.Intereses
            );

            TempData["Mensaje"] = result.Descripcion;

            return RedirectToAction("MisDatos");
        }
        [MenuAuthorize]
        public IActionResult Pago()
        {
            return View();
        }
        [MenuAuthorize]
        public IActionResult CambiodeClave()
        {
            return View(new CambioClaveViewModel());
        }

        [HttpPost]
        [MenuAuthorize]
        public IActionResult CambiodeClave(CambioClaveViewModel model)
        {
            // ❌ VALIDACIONES BACKEND
            if (string.IsNullOrWhiteSpace(model.NuevaClave) ||
                string.IsNullOrWhiteSpace(model.ConfirmarClave))
            {
                ModelState.AddModelError("", "Todos los campos son obligatorios.");
                return View(model);
            }

            if (model.NuevaClave != model.ConfirmarClave)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                return View(model);
            }

            // ✅ SI TODO OK → LLAMAR REPO
            var usuarioId = Guid.Parse(User.FindFirst("UsuarioId").Value);

            var resultado = _usuarioRepository.ActualizarClave(usuarioId, model.NuevaClave);

            TempData["Mensaje"] = "Clave actualizada correctamente";

            return RedirectToAction("CambiodeClave");
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

