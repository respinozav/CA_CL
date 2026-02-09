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

            var menuPlanoItem = _menuRepo.ObtenerMenuPorRol(result.RolId.Value);

            // mapear MenuItem → MenuDto
            var menuPlano = menuPlanoItem.Select(m => new MenuDto
            {
                MenuId = m.MenuId,
                MenuPadreId = m.MenuPadreId,
                Nombre = m.Nombre,
                Ruta = m.Ruta,
                Icono = m.Icono,
                Orden = m.Orden
            }).ToList();

            // construir jerarquía
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

            // guardar en sesión
            HttpContext.Session.SetString(
                "Menu",
                JsonSerializer.Serialize(menuJerarquico)
            );


            // guardar en sesión
            HttpContext.Session.SetString(
                "Menu",
                JsonSerializer.Serialize(menuJerarquico)
            );


            return RedirectToAction("Index", "Home");
        }

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

            if (result == null || result.Codigo != "USER_CREATED")
            {
                model.MensajeError = result?.Descripcion ?? "No fue posible registrar el usuario";
                return View(model);
            }

            model.MensajeOk = "Cuenta creada correctamente. Ahora puedes iniciar sesión.";
            ModelState.Clear();

            return View(new RegisterViewModel { MensajeOk = model.MensajeOk });
        }


    }
}
