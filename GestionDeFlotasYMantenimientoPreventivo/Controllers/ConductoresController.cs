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
    public class ConductoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConductoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Conductores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conductor>>> GetConductores()
        {
            return await _context.Conductores
                .Include(c => c.Licencias)
                .Include(c => c.Camion)
                .ToListAsync();
        }


        // GET: api/Conductores/5
        // GET: api/Conductores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conductor>> GetConductor(int id)
        {
            var conductor = await _context.Conductores
                .Include(c => c.Camion)      // Incluye los camiones del conductor
                .Include(c => c.Licencias)     // Incluye las licencias del conductor
                .FirstOrDefaultAsync(c => c.Codigo == id); // Encuentra el conductor por su ID

            if (conductor == null)
            {
                return NotFound();
            }

            return conductor;
        }


        // PUT: api/Conductores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConductor(int id, Conductor conductor)
        {
            if (id != conductor.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(conductor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConductorExists(id))
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

        // POST: api/Conductores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Conductor>> PostConductor(Conductor conductor)
        {
            _context.Conductores.Add(conductor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConductor", new { id = conductor.Codigo }, conductor);
        }

        // DELETE: api/Conductores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConductor(int id)
        {
            var conductor = await _context.Conductores.FindAsync(id);
            if (conductor == null)
            {
                return NotFound();
            }

            _context.Conductores.Remove(conductor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConductorExists(int id)
        {
            return _context.Conductores.Any(e => e.Codigo == id);
        }
    }
}
