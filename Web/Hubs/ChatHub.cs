using Microsoft.AspNetCore.SignalR;
using Web.Data;
using Web.Models;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, UsuarioDTO> usuariosOnline = new();
        private static Dictionary<string, string> conexiones = new();
        // key: userId, value: connectionId
        private readonly ChatRepository _chatRepo;
        public ChatHub(ChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }
        public async Task EnviarMensaje(string usuario, string mensaje)
        {
            var usuarioId = Guid.Parse(
                Context.GetHttpContext().Session.GetString("UsuarioId")
            );

            // 🔥 GUARDAR EN BD
            _chatRepo.GuardarMensajeGrupal(usuarioId, mensaje);

            await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje);
        }
        public async Task RegistrarUsuario(string nombre)
        {
            var userId = Context.GetHttpContext().Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(userId))
                return;

            usuariosOnline[Context.ConnectionId] = new UsuarioDTO
            {
                UsuarioId = userId,
                Nombre = nombre
            };

            await Clients.All.SendAsync("UsuariosConectados", usuariosOnline.Values);
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (usuariosOnline.ContainsKey(Context.ConnectionId))
            {
                usuariosOnline.Remove(Context.ConnectionId);
            }

            await Clients.All.SendAsync("UsuariosConectados", usuariosOnline.Values);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task UsuarioEscribiendo(string usuario)
        {
            await Clients.Others.SendAsync("MostrarEscribiendo", usuario);
        }
        public async Task EnviarMensajePrivadoPorNombres(string deUsuario, string paraUsuario, string mensaje)
        {
            var deId = usuariosOnline.FirstOrDefault(x => x.Value.Nombre == deUsuario).Value.UsuarioId;
            var paraId = usuariosOnline.FirstOrDefault(x => x.Value.Nombre == paraUsuario).Value.UsuarioId;

            Guid.TryParse(deId, out Guid deIdGuid);
            Guid.TryParse(paraId, out Guid paraIdGuid);

            await _chatRepo.GuardarMensaje(deIdGuid, paraIdGuid, mensaje);

            var paraIdStr = paraIdGuid.ToString();

            if (conexiones.TryGetValue(paraIdStr, out var connectionIdDestino))
            {
                await Clients.Client(connectionIdDestino)
                    .SendAsync("RecibirMensajePrivado", deIdGuid, paraIdGuid, mensaje);
            }

            // enviar también al que envía
            await Clients.Caller
                .SendAsync("RecibirMensajePrivado", deIdGuid, paraIdGuid, mensaje);
        }
        public async Task EnviarMensajePrivadoPorIds(Guid deId, Guid paraId, string mensaje)
        {
            await _chatRepo.GuardarMensaje(deId, paraId, mensaje);

            var paraIdStr = paraId.ToString();

            if (conexiones.TryGetValue(paraIdStr, out var connectionIdDestino))
            {
                await Clients.Client(connectionIdDestino)
                    .SendAsync("RecibirMensajePrivado", deId, paraId, mensaje);
            }

            // enviar también al que envía
            await Clients.Caller
                .SendAsync("RecibirMensajePrivado", deId, paraId, mensaje);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Session.GetString("UsuarioId");

            if (!string.IsNullOrEmpty(userId))
            {
                conexiones[userId] = Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }
    }
}