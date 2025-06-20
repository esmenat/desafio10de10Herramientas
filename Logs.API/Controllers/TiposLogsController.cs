using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logs.API.Models;

namespace Logs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposLogsController : ControllerBase
    {
        private readonly DbContext _context;

        public TiposLogsController(DbContext context)
        {
            _context = context;
        }

        // GET: api/TiposLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoLog>>> GetTipoLog()
        {
            return await _context.TipoLog.ToListAsync();
        }

        // GET: api/TiposLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoLog>> GetTipoLog(int id)
        {
            // Incluir los registros relacionados con el TipoLog
            var tipoLog = await _context.TipoLog
                .Include(t => t.Registros)  // Incluir los registros asociados al TipoLog
                .FirstOrDefaultAsync(t => t.Codigo == id);  // Encuentra el TipoLog por su ID

            if (tipoLog == null)
            {
                return NotFound();
            }

            return tipoLog;
        }

        // PUT: api/TiposLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoLog(int id, TipoLog tipoLog)
        {
            if (id != tipoLog.Codigo)
            {
                return BadRequest();
            }

            _context.Entry(tipoLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoLogExists(id))
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

        // POST: api/TiposLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoLog>> PostTipoLog(TipoLog tipoLog)
        {
            _context.TipoLog.Add(tipoLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoLog", new { id = tipoLog.Codigo }, tipoLog);
        }

        // DELETE: api/TiposLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoLog(int id)
        {
            var tipoLog = await _context.TipoLog.FindAsync(id);
            if (tipoLog == null)
            {
                return NotFound();
            }

            _context.TipoLog.Remove(tipoLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoLogExists(int id)
        {
            return _context.TipoLog.Any(e => e.Codigo == id);
        }
    }
}
