using System.Net;
using Gestion.API.Consumer;
using Logs.API.Models;
using Sistema.Modelos;
using Sistema.Modelos.Modelos;
using Sistema.MVC.Controllers;

namespace Sistema.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de endpoints
            Crud<Camion>.EndPoint = "https://localhost:7253/api/camiones";
            Crud<Conductor>.EndPoint = "https://localhost:7253/api/conductores";
            Crud<MantenimientoProgramado>.EndPoint = "https://localhost:7253/api/mantenimientosprogramadores";
            Crud<Taller>.EndPoint = "https://localhost:7253/api/talleres";
            Crud<Licencia>.EndPoint = "https://localhost:7253/api/licencias";
            Crud<TipoLog>.EndPoint = "https://localhost:7081/api/tiposlogs";
            Crud<RegistroLog>.EndPoint = "https://localhost:7081/api/registroslogs";

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Configuración de HttpClient (sin comentario)
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
            app.UseSession();
            app.UseAuthorization();

            // Rutas y controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            // Run the app
            app.Run();
        }
    }
}
