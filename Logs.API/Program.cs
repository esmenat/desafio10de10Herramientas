using System;
using Gestion.API.Consumer;
using Logs.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sistema.Modelos.Modelos;
using Sistema.Modelos;
using Logs.API.Controllers;

namespace Logs.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Crud<Camion>.EndPoint = "https://localhost:7253/api/camiones";
            Crud<MantenimientoProgramado>.EndPoint = "https://localhost:7253/api/MantenimientosProgramadores";
            Crud<Taller>.EndPoint = "https://localhost:7253/api/talleres";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DbContext>(options =>
                 options.UseMySql(
                  builder.Configuration.GetConnectionString("DbContext") ??
                    throw new InvalidOperationException("Connection string 'DbContext' not found."),
                        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbContext"))
                   )
             );

            builder.Services
              .AddControllers()
              .AddNewtonsoftJson(
                  options => options.SerializerSettings.ReferenceLoopHandling
                  = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<RegistrosLogs>();

            var app = builder.Build();
            CreateDefaultTipoLogs(app);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        // Método que verifica la existencia de tipos de log y los crea si no existen
        private static void CreateDefaultTipoLogs(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

                // Lista de tipos de log predeterminados
                var tipoLogs = new List<TipoLog>
                {
                    new TipoLog { NombreTipo = "BAJA-GASOLINA" },
                    new TipoLog { NombreTipo = "TEMPERATURA-ELEVADA" },
                    new TipoLog { NombreTipo = "MOTOR-DAÑADO" },
                    new TipoLog { NombreTipo = "KILOMETRAJE" },
                    new TipoLog { NombreTipo = "BAJO-NIVEL-ACEITE" },
                    new TipoLog { NombreTipo = "FALLA-SENSOR" }
                };

                // Verificar si cada tipo de log ya existe y agregarlo si no existe
                foreach (var tipo in tipoLogs)
                {
                    var existingTipoLog = dbContext.Set<TipoLog>().FirstOrDefault(t => t.NombreTipo == tipo.NombreTipo);
                    if (existingTipoLog == null)
                    {
                        dbContext.Set<TipoLog>().Add(tipo);
                    }
                }

                // Guardar cambios en la base de datos
                dbContext.SaveChanges();
            }
        }
    }
}
    