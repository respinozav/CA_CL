using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Filters;

namespace Web.Controllers
{

    [MenuAuthorize] // usa tu filtro de seguridad
    public class ChatController : Controller
    {
        private readonly ChatRepository _chatRepo;

        public ChatController(ChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }
        // 👇 ESTE ES PARA LA VISTA
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Inbox() // ESTA VISTA VAMOS A OCUPAR PARA EL CHAT PRIVADO
        {
            return View();
        }
        [HttpGet]
        public IActionResult ObtenerHistorial(int top = 50, DateTime? fechaAnterior = null)
        {
            var data = _chatRepo.ObtenerHistorial(top, fechaAnterior);
            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerInbox()
        {
            var usuarioId = Guid.Parse(HttpContext.Session.GetString("UsuarioId"));

            var data = await _chatRepo.ObtenerInbox(usuarioId);

            return Json(data);
        }
    }

}
