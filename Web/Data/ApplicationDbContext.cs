using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 👉 Aquí van tus tablas
        // public DbSet<Usuario> Usuarios { get; set; }
        // public DbSet<Post> Posts { get; set; }
    }
}
