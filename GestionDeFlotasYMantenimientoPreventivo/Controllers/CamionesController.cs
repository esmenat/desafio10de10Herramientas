using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Modelos;

namespace GestionMaestros.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CamionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Camiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camion>>> GetCamiones()
        {
            return await _context.Camiones
                .Include(c => c.MantenimientosProgramados)
                .Include(c => c.Conductores)
                .ToListAsync();
        }

        // GET: api/Camiones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Camion>> GetCamion(int id)
        {
            var camion = await _context.Camiones
                .Include(c => c.MantenimientosProgramados) // Incluye los mantenimientos programados
                .Include(c => c.Conductores)               // Incluye los conductores
                .FirstOrDefaultAsync(c => c.Codigo == id); // Encuentra el camión por su ID

            if (camion == null)
            {
                return NotFound();
            }

            return camion;
        }

        // PUT: api/Camiones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamion(int id, Camion camion)
        {
            if (id != camion.Codigo)
            {
                return BadRequest();
            }

            // Obtener el camión existente de la base de datos
            var existingCamion = await _context.Camiones.FindAsync(id);
            if (existingCamion == null)
            {
                return NotFound();
            }

            // Verificar si la placa ha cambiado y si es diferente
            if (existingCamion.Placa != camion.Placa)
            {
                return BadRequest("No se puede modificar la placa del camión.");
            }

            // Actualizar las propiedades del camión existente con los valores del camión enviado
            existingCamion.Modelo = camion.Modelo;
            existingCamion.Anio = camion.Anio;
            existingCamion.KilometrajeActual = camion.KilometrajeActual;
            existingCamion.Estado = camion.Estado;
            // Aquí puedes agregar más propiedades que se deben actualizar

            // Marcar la entidad como modificada
            _context.Entry(existingCamion).State = EntityState.Modified;

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CamionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Respuesta estándar para éxito con PUT
        }



        // POST: api/Camiones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Camion>> PostCamion(Camion camion)
        {
            _context.Camiones.Add(camion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCamion", new { id = camion.Codigo }, camion);
        }

        // DELETE: api/Camiones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCamion(int id)
        {
            var camion = await _context.Camiones.FindAsync(id);
            if (camion == null)
            {
                return NotFound();
            }

            _context.Camiones.Remove(camion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("Get-By-Placa/{placa}")]
        public async Task<IActionResult> GetCamionesByPlaca(string placa)
        {
            var camion = await _context.Camiones
                .FirstOrDefaultAsync(c => c.Placa == placa);  // Buscar el primer camión que coincida con la placa

            if (camion == null)
            {
                return NotFound();  // Si no se encuentra ningún camión, devolver 404
            }

            return Ok(camion);  // Devolver el camión encontrado
        }

        private bool CamionExists(int id)
        {
            return _context.Camiones.Any(e => e.Codigo == id);
        }
    }
}
