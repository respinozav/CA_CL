using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class LoginRepository
    {
        private readonly string _connectionString;

        public LoginRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public LoginResultDto LoginPorEmail(string email, string clave, string ip)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("app.sp_LoginUsuario", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Clave", clave);
            cmd.Parameters.AddWithValue("@IpOrigen", ip ?? string.Empty);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                throw new Exception("sp_LoginUsuario no retornó resultados.");

            return new LoginResultDto
            {
                Codigo = reader["Codigo"]?.ToString()!,
                Descripcion = reader["Descripcion"]?.ToString()!,

                UsuarioId = reader["UsuarioId"] == DBNull.Value
                    ? null
                    : (Guid)reader["UsuarioId"],

                RolId = reader["RolId"] == DBNull.Value
                    ? null
                    : (Guid)reader["RolId"],

                Nombre = reader["Nombre"] == DBNull.Value
                    ? null
                    : reader["Nombre"].ToString(),

                Email = reader["Email"] == DBNull.Value
                    ? null
                    : reader["Email"].ToString(),

                RolNombre = reader["RolNombre"] == DBNull.Value
                    ? null
                    : reader["RolNombre"].ToString()
            };
        }
    }
}
