using betsecrets.Modelos;
using Microsoft.EntityFrameworkCore;

namespace betsecrets.ORM
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios_Bet { get; set; }

    }
}
