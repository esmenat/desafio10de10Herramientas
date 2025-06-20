using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Modelos;

namespace GestionMaestros.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiesgoFalloController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RiesgoFalloController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/Camiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camion>>> GetPosiblesFallos()
        {
            var camiones = await _context.Camiones
                .Include(c => c.MantenimientosProgramados)
                .ToListAsync();
            var posiblesFallos = new List<Camion>();
          
            foreach (var camion in camiones)
            {
                if(camion.MantenimientosProgramados == null && camion.KilometrajeActual >= 10000)

                {
                    posiblesFallos.Add(camion); // Si no hay mantenimientos programados y el kilometraje es alto, se añade a la lista de posibles fallos
                    continue; // Si no hay mantenimientos programados, no hay riesgo de fallo
                }
                if(camion.MantenimientosProgramados == null )
                {
                    continue; // Si no hay mantenimientos programados y el kilometraje es bajo, no hay riesgo de fallo
                }
                var ultimoKilometraje = camion.MantenimientosProgramados
                    .OrderByDescending(m => m.FechaRealizada)
                    .Select(m => m.Kilometraje)
                    .FirstOrDefault();
                if ((camion.KilometrajeActual - ultimoKilometraje) >= 10000)
                {
                    posiblesFallos.Add(camion); // Si el kilometraje actual supera el último mantenimiento por más de 1000 km, se añade a la lista de posibles fallos
                }
            }
            return Ok(posiblesFallos); ;
        }
    }
}
