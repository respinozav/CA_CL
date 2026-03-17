using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class CiudadesRepository
    {
        private readonly IConfiguration _config;

        public CiudadesRepository(IConfiguration config)
        {
            _config = config;
        }

        public List<CiudadDto> ListarCiudades()
        {
            var lista = new List<CiudadDto>();

            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("app.sp_ListCiudades", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new CiudadDto
                {
                    CiudadId = Guid.Parse(reader["CiudadId"].ToString()),
                    Nombre = reader["NombreCiudad"].ToString()
                });
            }

            return lista;
        }
    }
}