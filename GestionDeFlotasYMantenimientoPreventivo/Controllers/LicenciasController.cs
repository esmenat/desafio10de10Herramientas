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
    public class LicenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LicenciasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Licencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Licencia>>> GetLicencias()
        {
            return await _context.Licencias
                .Include(l => l.Conductor) // Include the related Conductor entity
                .ToListAsync();
        }

        // GET: api/Licencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Licencia>> GetLicencia(int id)
        {
            var licencia = await _context.Licencias.FindAsync(id);

            if (licencia == null)
            {
                return NotFound();
            }

            return licencia;
        }

        // PUT: api/Licencias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLicencia(int id, Licencia licencia)
        {
            if (id != licencia.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(licencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LicenciaExists(id))
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

        // POST: api/Licencias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Licencia>> PostLicencia(Licencia licencia)
        {
            _context.Licencias.Add(licencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLicencia", new { id = licencia.Codigo }, licencia);
        }

        // DELETE: api/Licencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicencia(int id)
        {
            var licencia = await _context.Licencias.FindAsync(id);
            if (licencia == null)
            {
                return NotFound();
            }

            _context.Licencias.Remove(licencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LicenciaExists(int id)
        {
            return _context.Licencias.Any(e => e.Codigo == id);
        }
    }
}
