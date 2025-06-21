using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos; // Tu modelo de Usuario
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;
using Sistema.MVC.Data;

public class LoginController : Controller
{
    private readonly MiAppMVCContext _context; // Tu contexto de base de datos

    public LoginController(MiAppMVCContext context)
    {
        _context = context;
    }

    // Acción GET para mostrar el formulario de login
    public IActionResult Index()
    {
        return View();
    }

    // Acción POST para validar el usuario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string nombreUsuario, string contrasena)
    {
        // Verificar que los campos no estén vacíos
        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
        {
            ModelState.AddModelError("", "Por favor ingrese su nombre de usuario y contraseña.");
            return View();
        }

        // Buscar el usuario en la base de datos
        var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

        if (usuario == null || usuario.Contraseña != contrasena) // Aquí deberías usar un sistema seguro para comparar contraseñas
        {
            ViewData["ErrorMessage"] = "Credenciales inválidas";
            return View();
        }

        // Crear las claims para la autenticación
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.NameIdentifier, usuario.Codigo.ToString()) // Cambiar "Id" por "Codigo"
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Iniciar sesión
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        return RedirectToAction("Index", "Home"); // Redirigir a la página principal
    }

    // Acción para cerrar sesión
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home"); // Redirigir a la página principal
    }
}
