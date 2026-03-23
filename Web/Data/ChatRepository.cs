using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class ChatRepository
    {
        private readonly string _connectionString;

        public ChatRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void GuardarMensajeGrupal(Guid usuarioId, string mensaje)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("web.sp_GuardarMensajeGrupal", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Mensaje", mensaje);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<ChatGrupalViewModel> ObtenerHistorial(int top, DateTime? fechaAnterior)
        {
            var lista = new List<ChatGrupalViewModel>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("web.sp_ObtenerHistorialChatGrupal", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Top", top);
            cmd.Parameters.AddWithValue("@FechaAnterior", (object?)fechaAnterior ?? DBNull.Value);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ChatGrupalViewModel
                {
                    HistoriaChatId = (Guid)reader["HistoriaChatId"],
                    UsuarioId = (Guid)reader["UsuarioId"],
                    Nombre = reader["Nombre"].ToString(),
                    Mensaje = reader["Mensaje"].ToString(),
                    FechaEnvio = (DateTime)reader["FechaEnvio"]
                });
            }

            return lista.OrderBy(x => x.FechaEnvio).ToList();
        }
        public async Task GuardarMensaje(Guid deId, Guid paraId, string mensaje)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("web.sp_Chat_InsertarMensaje", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DeUsuarioId", deId);
            cmd.Parameters.AddWithValue("@ParaUsuarioId", paraId);
            cmd.Parameters.AddWithValue("@Mensaje", mensaje);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<dynamic>> ObtenerInbox(Guid usuarioId)
        {
            var lista = new List<dynamic>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("web.sp_Chat_ObtenerInbox", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new
                {
                    UsuarioId = (Guid)reader["UsuarioId"],
                    Nombre = reader["Nombre"].ToString(),
                    UltimoMensaje = reader["UltimoMensaje"].ToString(),
                    Fecha = (DateTime)reader["Fecha"]
                });
            }

            return lista;
        }
    }
}