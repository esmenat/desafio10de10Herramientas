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
    public class TalleresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TalleresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Talleres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Taller>>> GetTallers()
        {
            return await _context.Tallers
                .Include(c => c.MantenimientosProgramados) // Include the related MantenimientosProgramados entity
                .ToListAsync();
        }

        // GET: api/Talleres/5
        // GET: api/Talleres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Taller>> GetTaller(int id)
        {
            // Incluir los mantenimientos relacionados con el taller
            var taller = await _context.Tallers
                .Include(t => t.MantenimientosProgramados) // Asegúrate de que 'Mantenimientos' es una propiedad de navegación en el modelo Taller
                .FirstOrDefaultAsync(t => t.Codigo == id); // Encuentra el taller por su ID

            if (taller == null)
            {
                return NotFound();
            }

            return taller;
        }


        // PUT: api/Talleres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaller(int id, Taller taller)
        {
            if (id != taller.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(taller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TallerExists(id))
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

        // POST: api/Talleres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Taller>> PostTaller(Taller taller)
        {
            _context.Tallers.Add(taller);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaller", new { id = taller.Codigo }, taller);
        }

        // DELETE: api/Talleres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaller(int id)
        {
            var taller = await _context.Tallers.FindAsync(id);
            if (taller == null)
            {
                return NotFound();
            }

            _context.Tallers.Remove(taller);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TallerExists(int id)
        {
            return _context.Tallers.Any(e => e.Codigo == id);
        }
    }
}
