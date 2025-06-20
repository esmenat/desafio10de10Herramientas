using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sistema.Modelos.Modelos;

public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sistema.Modelos.Camion> Camiones { get; set; } = default!;

public DbSet<Conductor> Conductores { get; set; } = default!;

public DbSet<Licencia> Licencias { get; set; } = default!;

public DbSet<MantenimientoProgramado> MantenimientosProgramados { get; set; } = default!;

public DbSet<Taller> Tallers { get; set; } = default!;

public DbSet<Usuario> Usuarios { get; set; } = default!;


}
