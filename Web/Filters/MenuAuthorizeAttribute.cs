using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Web.Models;

namespace Web.Filters
{
    public class MenuAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;

            // 1️⃣ Validar sesión
            if (string.IsNullOrEmpty(session.GetString("RolId")))
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Authentication",
                    null
                );
                return;
            }

            // 2️⃣ Obtener menú desde Session
            var menuJson = session.GetString("Menu");
            if (string.IsNullOrEmpty(menuJson))
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

            var menu = JsonSerializer.Deserialize<List<MenuItem>>(menuJson);

            // 3️⃣ Ruta actual
            var rutaActual = context.HttpContext.Request.Path.Value?
                .ToLower()
                .TrimEnd('/');

            if (string.IsNullOrEmpty(rutaActual))
                return;

            // 4️⃣ Validar acceso
            var tieneAcceso = menu!.Any(m =>
                !string.IsNullOrEmpty(m.Ruta) &&
                m.Ruta.ToLower().TrimEnd('/') == rutaActual
            );

            if (!tieneAcceso)
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
