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
    public class MantenimientosProgramadoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MantenimientosProgramadoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MantenimientosProgramadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MantenimientoProgramado>>> GetMantenimientosProgramados()
        {
            return await _context.MantenimientosProgramados
                .Include(c => c.Camion) // Include the related Camion entity
                .Include(c => c.Taller) // Include the related Conductor entity
                .ToListAsync();
        }

        // GET: api/MantenimientosProgramadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MantenimientoProgramado>> GetMantenimientoProgramado(int id)
        {
            var mantenimientoProgramado = await _context.MantenimientosProgramados.FindAsync(id);

            if (mantenimientoProgramado == null)
            {
                return NotFound();
            }

            return mantenimientoProgramado;
        }
       

        // PUT: api/MantenimientosProgramadores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMantenimientoProgramado(int id, MantenimientoProgramado mantenimientoProgramado)
        {
            if (id != mantenimientoProgramado.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(mantenimientoProgramado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MantenimientoProgramadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MantenimientosProgramadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MantenimientoProgramado>> PostMantenimientoProgramado(MantenimientoProgramado mantenimientoProgramado)
        {
            _context.MantenimientosProgramados.Add(mantenimientoProgramado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMantenimientoProgramado", new { id = mantenimientoProgramado.Codigo }, mantenimientoProgramado);
        }

        // DELETE: api/MantenimientosProgramadores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMantenimientoProgramado(int id)
        {
            var mantenimientoProgramado = await _context.MantenimientosProgramados.FindAsync(id);
            if (mantenimientoProgramado == null)
            {
                return NotFound();
            }

            _context.MantenimientosProgramados.Remove(mantenimientoProgramado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // GET: api/MantenimientosProgramadores
        [HttpGet("alerta")]
        public async Task<ActionResult<IEnumerable<MantenimientoProgramado>>> GetMantenimientosProgramadosAlertas()
        {
            return await _context.MantenimientosProgramados
                .Include(c => c.Camion)
                .Include(c => c.Taller)
                .Where(mp => mp.Estado == MantenimientoProgramado.EstadoMantenimiento.Pendiente)
                .ToListAsync();
        }
        private bool MantenimientoProgramadoExists(int id)
        {
            return _context.MantenimientosProgramados.Any(e => e.Codigo == id);
        }
        [HttpGet("ultimo-mantenimiento/{codigoCamion}")]
        public async Task<ActionResult<MantenimientoProgramado>> GetUltimoMantenimiento(int codigoCamion)
        {
            // Filtrar mantenimientos completados y ordenarlos por la fecha de realización (de más reciente a más antigua)
            var ultimoMantenimiento = await _context.MantenimientosProgramados
                .Where(m => m.CamionCodigo == codigoCamion && m.Estado == MantenimientoProgramado.EstadoMantenimiento.Finalizado) // Filtrar por camión y estado "Finalizado"
                .OrderByDescending(m => m.FechaRealizada)  // Ordenar por fecha de realización descendente
                .FirstOrDefaultAsync();  // Obtener el primer registro (el más reciente)

            if (ultimoMantenimiento == null)
            {
                return NotFound();  // Si no se encuentra ningún mantenimiento, devolver 404
            }

            return Ok(ultimoMantenimiento);  // Devolver el mantenimiento encontrado
        }

    }
}
