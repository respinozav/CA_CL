using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly LoginRepository _loginRepo;
        private readonly MenuRepository _menuRepo;

        public AuthenticationController(
            LoginRepository loginRepo,
            MenuRepository menuRepo)
        {
            _loginRepo = loginRepo;
            _menuRepo = menuRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 🔐 LOGIN POR EMAIL
            var result = _loginRepo.LoginPorEmail(model.Email, model.Clave, ip);

            // ❌ Login fallido
            if (result == null || result.Codigo != "LOGIN_OK")
            {
                model.MensajeError = result?.Descripcion ?? "No fue posible iniciar sesión";
                return View(model);
            }

            // ✅ Login exitoso → Session
            HttpContext.Session.SetString("UsuarioId", result.UsuarioId!.Value.ToString());
            HttpContext.Session.SetString("RolId", result.RolId!.Value.ToString());
            HttpContext.Session.SetString("Nombre", result.Nombre!);
            HttpContext.Session.SetString("Email", result.Email!);

            var menu = _menuRepo.ObtenerMenuPorRol(result.RolId.Value);
            HttpContext.Session.SetString(
                "Menu",
                JsonSerializer.Serialize(menu)
            );

            return RedirectToAction("Index", "Dashboard");
        }

  
        public IActionResult Registretion()
        {
            return View();
        }
    }
}
