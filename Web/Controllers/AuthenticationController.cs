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
        private readonly RegisterRepository _registerRepo;

        public AuthenticationController(
            LoginRepository loginRepo,
            MenuRepository menuRepo,
            RegisterRepository registerRepo)
        {
            _loginRepo = loginRepo;
            _menuRepo = menuRepo;
            _registerRepo = registerRepo;
        }

        /* =====================================================
           LOGIN
        ===================================================== */

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

            var result = _loginRepo.LoginPorEmail(model.Email, model.Clave, ip);

            if (result == null || result.Codigo != "LOGIN_OK")
            {
                model.MensajeError = result?.Descripcion ?? "No fue posible iniciar sesión.";
                return View(model);
            }

            // 🔐 Guardar datos en sesión
            HttpContext.Session.SetString("UsuarioId", result.UsuarioId!.Value.ToString());
            HttpContext.Session.SetString("RolId", result.RolId!.Value.ToString());
            HttpContext.Session.SetString("Nombre", result.Nombre!);
            HttpContext.Session.SetString("Email", result.Email!);

            // Construcción del menú jerárquico
            var menuPlanoItem = _menuRepo.ObtenerMenuPorRol(result.RolId.Value);

            var menuPlano = menuPlanoItem.Select(m => new MenuDto
            {
                MenuId = m.MenuId,
                MenuPadreId = m.MenuPadreId,
                Nombre = m.Nombre,
                Ruta = m.Ruta,
                Icono = m.Icono,
                Orden = m.Orden
            }).ToList();

            var menuJerarquico = menuPlano
                .Where(m => m.MenuPadreId == null)
                .Select(padre =>
                {
                    padre.Hijos = menuPlano
                        .Where(h => h.MenuPadreId == padre.MenuId)
                        .OrderBy(h => h.Orden)
                        .ToList();
                    return padre;
                })
                .OrderBy(m => m.Orden)
                .ToList();

            HttpContext.Session.SetString(
                "Menu",
                JsonSerializer.Serialize(menuJerarquico)
            );

            return RedirectToAction("Index", "Home");
        }

        /* =====================================================
           REGISTRO
        ===================================================== */

        [HttpGet]
        public IActionResult Registretion()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registretion(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _registerRepo.RegistrarUsuario(
                model.Usuario,
                model.Clave,
                model.Nombre,
                model.Email
            );

            if (result == null || result.Codigo != "USER_CREATED_PENDING_CONFIRMATION")
            {
                model.MensajeError = result?.Descripcion ?? "No fue posible registrar el usuario.";
                return View(model);
            }

            ModelState.Clear();

            return View(new RegisterViewModel
            {
                MensajeOk = "Revisa tu correo para confirmar tu cuenta antes de iniciar sesión."
            });
        }

        /* =====================================================
           CONFIRMAR EMAIL
        ===================================================== */

        [HttpGet]
        public IActionResult ConfirmarEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return View("TokenInvalido", "Token no válido.");

            var result = _registerRepo.ConfirmarEmail(token);

            if (result.Codigo == "EMAIL_CONFIRMED")
                return View("EmailConfirmado", result.Descripcion);

            return View("TokenInvalido",
                result.Descripcion ?? "El enlace no es válido o ha expirado.");
        }
    }
}
