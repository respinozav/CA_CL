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
        private readonly CiudadesRepository _ciudadesRepo;
        private readonly GenerosRepository _generosRepo;
        public AuthenticationController(
            LoginRepository loginRepo,
            MenuRepository menuRepo,
            RegisterRepository registerRepo,
            CiudadesRepository ciudadesRepo,
            GenerosRepository generosRepo)
        {
            _loginRepo = loginRepo;
            _menuRepo = menuRepo;
            _registerRepo = registerRepo;
            _ciudadesRepo = ciudadesRepo;
            _generosRepo = generosRepo;
        }
        private void CargarCombos(RegisterViewModel model)
        {
            model.Ciudades = _ciudadesRepo.ListarCiudades();
            model.Generos = _generosRepo.ListarGeneros();
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

            return RedirectToAction("MisDatos", "Home");
        }

        /* =====================================================
            REGISTRO
         ===================================================== */

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            CargarCombos(model); // 🔥 ESTA ES LA CLAVE

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            // 🔁 Siempre recargar combos
            CargarCombos(model);

            // ❌ Validaciones automáticas (DataAnnotations)
            if (!ModelState.IsValid)
                return View(model);

            // 🔞 Edad
            if (!model.EsMayorDeEdad())
            {
                model.MensajeError = "Debes ser mayor de 18 años para registrarte.";
                return View(model);
            }

            // ❌ Ciudad
            if (model.CiudadId == Guid.Empty)
            {
                model.MensajeError = "Debes seleccionar una ciudad.";
                return View(model);
            }

            // ❌ Género
            if (model.GeneroId == Guid.Empty)
            {
                model.MensajeError = "Debes seleccionar un género.";
                return View(model);
            }

            // 🚀 REGISTRO (AHORA CON GENEROID)
            var result = _registerRepo.RegistrarUsuario(
                model.Usuario,
                model.Clave,
                model.Nombre,
                model.Email,
                model.FechaNacimiento,
                model.CiudadId,
                model.GeneroId,   // 🔥 AQUÍ ESTÁ EL CAMBIO CLAVE
                model.Intereses ?? ""
            );

            if (result == null || result.Codigo != "USER_CREATED_PENDING_CONFIRMATION")
            {
                model.MensajeError = result?.Descripcion ?? "No fue posible registrar el usuario.";
                return View(model);
            }

            // 🧹 limpiar modelo
            ModelState.Clear();

            var nuevoModelo = new RegisterViewModel();
            CargarCombos(nuevoModelo);

            nuevoModelo.MensajeOk = "Revisa tu correo para confirmar tu cuenta antes de iniciar sesión.";

            return View(nuevoModelo);
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
