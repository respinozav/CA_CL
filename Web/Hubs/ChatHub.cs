using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> usuariosOnline = new();

        public async Task RegistrarUsuario(string usuario)
        {
            usuariosOnline[Context.ConnectionId] = usuario;

            await Clients.All.SendAsync("UsuariosConectados", usuariosOnline.Values);
        }

        public async Task EnviarMensaje(string usuario, string mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", usuario, mensaje);
        }
        public async Task EnviarMensajePrivado(string deUsuario, string paraUsuario, string mensaje)
        {
            var destinoConnectionId = usuariosOnline
                .FirstOrDefault(x => x.Value == paraUsuario).Key;

            var origenConnectionId = usuariosOnline
                .FirstOrDefault(x => x.Value == deUsuario).Key;

            if (destinoConnectionId != null)
            {
                await Clients.Client(destinoConnectionId)
                    .SendAsync("RecibirMensajePrivado", deUsuario, paraUsuario, mensaje);
            }

            if (origenConnectionId != null)
            {
                await Clients.Client(origenConnectionId)
                    .SendAsync("RecibirMensajePrivado", deUsuario, paraUsuario, mensaje);
            }
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
    }
}