using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Sistema.Modelos.Modelos;  // Asegúrate de importar AutoMapper

namespace GestionDeFlotasYMantenimientoPreventivo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de la base de datos
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")
                ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

            // Configuración de AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));  // Registra AutoMapper con el perfil de mapeo
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                 );
            // Agregar servicios HTTP Client para que MVC haga peticiones a la API
            builder.Services.AddHttpClient();  // Registrar HttpClient para la inyección de dependencias

            // Agregar servicios para los controladores
            builder.Services.AddControllers();

            // Configuración de Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuración de CORS (Permitir solicitudes desde MVC)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMvc", policy =>
                {
                    policy.WithOrigins("http://localhost:5000")  // Cambia esta URL por la de tu aplicación MVC
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();
            CreateDefaultUsuarioAdmin(app);  // Crear un usuario admin por defecto si no existe
            // Aplicar CORS
            app.UseCors("AllowMvc");

            // Configuración de Swagger solo en desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configuración del middleware
            app.UseHttpsRedirection();  // Redirigir a HTTPS
            app.UseAuthorization();  // Usar autorización

            // Mapear los controladores
            app.MapControllers();

            // Iniciar la aplicación
            app.Run();
        }
        private static void CreateDefaultUsuarioAdmin(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Verificar si ya existe un usuario admin
                var adminUsuario = dbContext.Usuarios.FirstOrDefault(u => u.NombreUsuario == "admin");

                if (adminUsuario == null)
                {
                    // Crear un nuevo usuario admin si no existe
                    var newAdmin = new Usuario
                    {
                        NombreUsuario = "admin",
                        Contraseña = "123456" // Reemplaza con un valor más seguro o un hash de contraseña
                    };

                    dbContext.Usuarios.Add(newAdmin);
                    dbContext.SaveChanges();
                }
            }
        }

    }
}
