using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public UsuarioViewModel ObtenerUsuario(Guid usuarioId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("app.sp_ObtenerUsuario", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UsuarioViewModel
                {
                    Usuario = reader["Usuario"].ToString(),
                    Nombre = reader["Nombre"].ToString(),
                    FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                    Ciudad = reader["Ciudad"].ToString(),
                    Genero = reader["Genero"].ToString(),
                    Intereses = reader["Intereses"].ToString(),
                    Email = reader["Email"].ToString(),
                    FechaCreacion = (DateTime)reader["FechaCreacion"]
                };
            }

            return null;
        }
    }
}