using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Modelos.Modelos;

namespace GestionMaestros.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }


       
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Codigo == id);
        }
       
        // POST: api/Usuarios/Login
        [HttpPost("Login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] Usuario loginUsuario)
        {
            // Validar que los datos del login no sean nulos o vacíos
            if (string.IsNullOrEmpty(loginUsuario.NombreUsuario) || string.IsNullOrEmpty(loginUsuario.Contraseña))
            {
                return BadRequest("El nombre de usuario o la contraseña no pueden estar vacíos.");
            }

            // Buscar el usuario en la base de datos por el nombre de usuario
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == loginUsuario.NombreUsuario);

            // Verificar si el usuario existe
            if (usuarioExistente == null)
            {
                return Unauthorized("Nombre de usuario no encontrado.");
            }

            // Verificar si la contraseña coincide
            if (usuarioExistente.Contraseña != loginUsuario.Contraseña)
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Si el nombre de usuario y la contraseña son correctos, devolver el usuario
            return Ok(usuarioExistente);
        }


    }
}
