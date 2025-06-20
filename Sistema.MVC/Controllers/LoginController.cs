using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace Sistema.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        // Constructor que recibe HttpClient (deberías configurarlo en ConfigureServices)
        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Acción GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // Acción POST: Login (Recibe las credenciales y las manda a la API)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(string nombreUsuario, string contrasena)
        //{
        //    // Validar que los datos no sean vacíos
        //    if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
        //    {
        //        ModelState.AddModelError("", "El nombre de usuario o la contraseña no pueden estar vacíos.");
        //        return View();
        //    }

        //    // Crear el objeto de login que se enviará al API
        //    var loginUsuario = new
        //    {
        //        NombreUsuario = nombreUsuario,
        //        Contraseña = contrasena
        //    };

        //    // Serializar el objeto loginUsuario en formato JSON
        //    var jsonContent = new StringContent(JsonConvert.SerializeObject(loginUsuario), Encoding.UTF8, "application/json");

        //    try
        //    {
        //        // Hacer una solicitud POST al API
        //        var response = await _httpClient.PostAsync("https://localhost:7253/api/Usuarios/Login", jsonContent);

        //        // Si la respuesta es exitosa, redirigir a la página principal
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index", "Home");  // Redirige a la página principal
        //        }
        //        else
        //        {
        //            // Si la respuesta no es exitosa, agregar un mensaje de error
        //            ViewData["ErrorMessage"] = "Usuario o Contraseña Incorrectos";
        //            return View();  // Vuelve a la vista de login si las credenciales son incorrectas
        //        }
        //    }
        //    catch (HttpRequestException httpEx)
        //    {
        //        // Capturar error relacionado con la solicitud HTTP
        //        ViewData["ErrorMessage"] = $"Error en la solicitud HTTP: {httpEx.Message}";
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Capturar cualquier otra excepción
        //        ViewData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
        //        return View();
        //    }
        //}
        // Acción POST: Login (Recibe las credenciales y las manda a la API)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string nombreUsuario, string contrasena)
        {
            // Validar que los datos no sean vacíos
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
            {
                ModelState.AddModelError("", "El nombre de usuario o la contraseña no pueden estar vacíos.");
                return View();
            }

            // Crear el objeto de login que se enviará al API
            var loginUsuario = new
            {
                NombreUsuario = nombreUsuario,
                Contraseña = contrasena
            };

            // Serializar el objeto loginUsuario en formato JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginUsuario), Encoding.UTF8, "application/json");

            try
            {
                // Hacer una solicitud POST al API
                var response = await _httpClient.PostAsync("https://localhost:7253/api/Usuarios/Login", jsonContent);

                // Si la respuesta es exitosa, guardar el estado de sesión y redirigir a la página principal
                if (response.IsSuccessStatusCode)
                {
                    // Establecer en la sesión que el usuario está autenticado
                    HttpContext.Session.SetString("UserLoggedIn", "true");

                    return RedirectToAction("Index", "Home");  // Redirige a la página principal
                }
                else
                {
                    // Si la respuesta no es exitosa, agregar un mensaje de error
                    ViewData["ErrorMessage"] = "Usuario o Contraseña Incorrectos";
                    return View();  // Vuelve a la vista de login si las credenciales son incorrectas
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Capturar error relacionado con la solicitud HTTP
                ViewData["ErrorMessage"] = $"Error en la solicitud HTTP: {httpEx.Message}";
                return View();
            }
            catch (Exception ex)
            {
                // Capturar cualquier otra excepción
                ViewData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                return View();
            }
        }

    }
}
