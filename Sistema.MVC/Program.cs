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

            // Configuraci�n de endpoints
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
                    options.LoginPath = "/Login/Index"; // P�gina de login
                });

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Configuraci�n de HttpClient
            builder.Services.AddHttpClient<RiesgoFalloController>();

            // Configuraci�n de almacenamiento en memoria y sesiones
            builder.Services.AddDistributedMemoryCache(); // Agregar almacenamiento en memoria
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Establecer el tiempo de expiraci�n de la sesi�n
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
                        Contrase�a = "123456", // Aseg�rate de usar un hash de la contrase�a en producci�n
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

            // Uso de routing y sesi�n
            app.UseRouting();
            app.UseAuthentication(); // Para usar autenticaci�n
            app.UseAuthorization(); // Para usar autorizaci�n

            // Rutas y controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            // Run the app
            app.Run();
        }
    }
}
