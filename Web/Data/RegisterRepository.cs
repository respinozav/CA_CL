using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class RegisterRepository
    {
        private readonly IConfiguration _config;

        public RegisterRepository(IConfiguration config)
        {
            _config = config;
        }

        public RegistroResultDto RegistrarUsuario(
            string usuario,
            string clave,
            string nombre,
            string email)
        {
            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            using var cmd = new SqlCommand("app.sp_RegistrarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parámetros tipados (mejor práctica)
            cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 200).Value = clave;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 150).Value = nombre;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new RegistroResultDto
                {
                    Codigo = reader["Codigo"]?.ToString() ?? string.Empty,
                    Descripcion = reader["Descripcion"]?.ToString() ?? string.Empty
                };
            }

            return new RegistroResultDto
            {
                Codigo = "UNKNOWN_ERROR",
                Descripcion = "No se obtuvo respuesta del servidor."
            };
        }

        public RegistroResultDto ConfirmarEmail(string token)
        {
            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            using var cmd = new SqlCommand("app.sp_ConfirmarEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Token", SqlDbType.NVarChar, 200).Value = token;

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new RegistroResultDto
                {
                    Codigo = reader["Codigo"]?.ToString() ?? string.Empty,
                    Descripcion = reader["Descripcion"]?.ToString() ?? string.Empty
                };
            }

            return new RegistroResultDto
            {
                Codigo = "INVALID_OR_EXPIRED_TOKEN",
                Descripcion = "El enlace no es válido o ha expirado."
            };
        }
    }
}