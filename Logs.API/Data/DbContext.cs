using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Logs.API.Models;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Logs.API.Models.RegistroLog> RegistroLog { get; set; } = default!;

public DbSet<Logs.API.Models.TipoLog> TipoLog { get; set; } = default!;
    }
