using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
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
            string email,
            DateTime fechaNacimiento,
            Guid ciudadId,
            Guid generoId,
            string intereses)
        {
            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            using var cmd = new SqlCommand("app.sp_RegistrarUsuario", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 200).Value = clave;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 150).Value = nombre;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;
            cmd.Parameters.Add("@FechaNacimiento", SqlDbType.Date).Value = fechaNacimiento;
            cmd.Parameters.Add("@CiudadId", SqlDbType.UniqueIdentifier).Value = ciudadId;
            cmd.Parameters.Add("@GeneroId", SqlDbType.UniqueIdentifier).Value = generoId;
            cmd.Parameters.Add("@Intereses", SqlDbType.NVarChar, 500).Value = intereses ?? "";

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