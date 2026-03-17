using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class GenerosRepository
    {
        private readonly IConfiguration _config;

        public GenerosRepository(IConfiguration config)
        {
            _config = config;
        }

        public List<GeneroDto> ListarGeneros()
        {
            var lista = new List<GeneroDto>();

            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            using var cmd = new SqlCommand("app.sp_ListGeneros", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new GeneroDto
                {
                    GeneroId = Guid.Parse(reader["GeneroId"].ToString()),
                    NombreGenero = reader["NombreGenero"].ToString()
                });
            }

            return lista;
        }
    }
}