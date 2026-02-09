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

            cmd.Parameters.AddWithValue("@Usuario", usuario);
            cmd.Parameters.AddWithValue("@Clave", clave);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new RegistroResultDto
                {
                    Codigo = reader["Codigo"].ToString(),
                    Descripcion = reader["Descripcion"].ToString()
                };
            }

            return null!;
        }
    }

}
