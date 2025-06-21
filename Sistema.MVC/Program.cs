using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Sistema.Modelos;
using Sistema.MVC.Controllers;
using Sistema.MVC.Data;
using Microsoft.Extensions.DependencyInjection;
using Gestion.API.Consumer;
using Logs.API.Models;
using Sistema.Modelos.Modelos;

namespace Sistema.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MiAppMVCContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")
                ?? throw new InvalidOperationException("Connection string 'MiAppMVCContext' not found.")));

            // Configuración de endpoints
            Crud<Camion>.EndPoint = "https://localhost:7253/api/camiones";
            Crud<Conductor>.EndPoint = "https://localhost:7253/api/conductores";
            Crud<MantenimientoProgramado>.EndPoint = "https://localhost:7253/api/mantenimientosprogramadores";
            Crud<Taller>.EndPoint = "https://localhost:7253/api/talleres";
            Crud<Licencia>.EndPoint = "https://localhost:7253/api/licencias";
            Crud<TipoLog>.EndPoint = "https://localhost:7081/api/tiposlogs";
            Crud<RegistroLog>.EndPoint = "https://localhost:7081/api/registroslogs";

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index"; // Página de login
                });

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Configuración de HttpClient
            builder.Services.AddHttpClient<RiesgoFalloController>();

            // Configuración de almacenamiento en memoria y sesiones
            builder.Services.AddDistributedMemoryCache(); // Agregar almacenamiento en memoria
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Establecer el tiempo de expiración de la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Build the app
            var app = builder.Build();

            // Crear el usuario admin si no existe
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MiAppMVCContext>();

                // Verificar si el usuario admin ya existe
                var adminUsuario = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == "admin");

                if (adminUsuario == null)
                {
                    // Crear un nuevo usuario admin si no existe
                    context.Usuarios.Add(new Usuario
                    {
                        NombreUsuario = "admin",
                        Contraseña = "123456", // Asegúrate de usar un hash de la contraseña en producción
                    });

                    context.SaveChanges(); // Guardar cambios
                }
            }

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Uso de routing y sesión
            app.UseRouting();
            app.UseAuthentication(); // Para usar autenticación
            app.UseAuthorization(); // Para usar autorización

            // Rutas y controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            // Run the app
            app.Run();
        }
    }
}
