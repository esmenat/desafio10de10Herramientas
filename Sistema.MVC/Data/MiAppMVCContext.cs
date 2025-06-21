using Microsoft.EntityFrameworkCore;
using Sistema.Modelos.Modelos;

namespace Sistema.MVC.Data
{
    public class MiAppMVCContext    :   DbContext
    {
     public MiAppMVCContext(DbContextOptions<MiAppMVCContext> options)
     : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
