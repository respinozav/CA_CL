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
                    CiudadId = reader["CiudadId"] != DBNull.Value
                        ? Guid.Parse(reader["CiudadId"].ToString())
                        : (Guid?)null,
                    GeneroId = reader["GeneroId"] != DBNull.Value
                        ? Guid.Parse(reader["GeneroId"].ToString())
                        : (Guid?)null,
                    Genero = reader["Genero"].ToString(),
                    Intereses = reader["Intereses"].ToString(),
                    Email = reader["Email"].ToString(),
                    FechaCreacion = (DateTime)reader["FechaCreacion"]
                };
            }

            return null;
        }
        public (string Codigo, string Descripcion) ActualizarUsuario(Guid usuarioId, string nombre, Guid? ciudadId, Guid? generoId, string intereses)
        {
            using var conn = new SqlConnection(_connectionString);

            using var cmd = new SqlCommand("app.sp_ActualizarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@CiudadId", (object?)ciudadId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GeneroId", (object?)generoId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Intereses", intereses ?? "");

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return (
                    reader["Codigo"].ToString(),
                    reader["Descripcion"].ToString()
                );
            }

            return ("ERROR", "No se pudo actualizar");
        }
        public (string Codigo, string Descripcion) ActualizarClave(Guid usuarioId, string clave)
        {
            using var conn = new SqlConnection(_connectionString);

            using var cmd = new SqlCommand("app.sp_ActualizarClave", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Clave", clave);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return (
                    reader["Codigo"].ToString(),
                    reader["Descripcion"].ToString()
                );
            }

            return ("ERROR", "No se pudo actualizar la clave");
        }
    }
}