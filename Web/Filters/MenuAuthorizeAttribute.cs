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
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }

            // 2️⃣ Obtener menú desde Session
            var menuJson = session.GetString("Menu");
            if (string.IsNullOrEmpty(menuJson))
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

            var menu = JsonSerializer.Deserialize<List<MenuDto>>(menuJson);
            if (menu == null)
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

            // 3️⃣ Ruta actual
            var rutaActual = context.HttpContext.Request.Path.Value?
                .ToLower()
                .TrimEnd('/');

            if (string.IsNullOrEmpty(rutaActual))
                return;

            // 4️⃣ Aplanar menú (padres + hijos)
            var rutasPermitidas = menu
                .SelectMany(m => new[] { m }.Concat(m.Hijos))
                .Where(m => !string.IsNullOrEmpty(m.Ruta))
                .Select(m => m.Ruta!.ToLower().TrimEnd('/'))
                .ToList();

            // 5️⃣ Validar acceso
            var tieneAcceso = rutasPermitidas.Any(r =>
                rutaActual == r || rutaActual.StartsWith(r)
            );

            if (!tieneAcceso)
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
