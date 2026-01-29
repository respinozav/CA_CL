using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Web.Data
{
    public class MenuRepository
    {
        private readonly string _connectionString;

        public MenuRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<MenuItem> ObtenerMenuPorRol(Guid rolId)
        {
            var menu = new List<MenuItem>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("app.sp_MenuPorRol", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RolId", rolId);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                menu.Add(new MenuItem
                {
                    MenuId = (Guid)reader["MenuId"],
                    MenuPadreId = reader["MenuPadreId"] == DBNull.Value
                        ? null
                        : (Guid)reader["MenuPadreId"],

                    Nombre = reader["Nombre"].ToString(),
                    Icono = reader["Icono"] == DBNull.Value
                        ? null
                        : reader["Icono"].ToString(),

                    Ruta = reader["Ruta"] == DBNull.Value
                        ? null
                        : reader["Ruta"].ToString(),

                    Orden = (int)reader["Orden"]
                });
            }

            return menu;
        }
    }
}
