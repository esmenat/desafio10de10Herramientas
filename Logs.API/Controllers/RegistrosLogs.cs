using System.Text.Json;
using Gestion.API.Consumer;
using Logs.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Sistema.Modelos;
using Sistema.Modelos.Modelos;

namespace Logs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosLogs : ControllerBase
    {
        private readonly DbContext _context;
        private readonly HttpClient _httpClient;
        public RegistrosLogs(DbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroLog>>> GetRegistroLog()
        {
            // Ordena por FechaHora de forma descendente para obtener el log más reciente primero
            return await _context.RegistroLog.OrderByDescending(d => d.FechaHora).ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<RegistroLog>> PostTipoLog(RegistroLog registroLog)
        {
            if (registroLog == null)
            {
                return BadRequest("El registro de log no puede ser nulo.");
            }

            registroLog.FechaHora = DateTime.Now;

            _context.RegistroLog.Add(registroLog);
            await _context.SaveChangesAsync();

            TipoLog? tipo = await _context.TipoLog.FindAsync(registroLog.TipoLogCodigo);
            if (tipo == null)
            {
                return NotFound("Tipo de log no encontrado.");
            }
            var camion = await GetCamionByPlaca(registroLog.PlacaCamion);
            if(camion == null)
            {
                return NotFound("Camión no encontrado.");
            }
            switch (tipo.NombreTipo)
            {
                case "BAJO-NIVEL-ACEITE":
                case "TEMPERATURA-ELEVADA":
                case "MOTOR-DAÑADO":
                case "FALLA-SENSOR":
                    var taller = GetRandomTaller();
                    if (taller == null)
                    {
                        return NotFound("No hay talleres disponibles.");
                    }
                    Crud<MantenimientoProgramado>.Create(
                        new MantenimientoProgramado
                        {
                            CamionCodigo = camion.Codigo,
                            TallerCodigo = taller.Codigo, // Asignar un taller por defecto o buscar uno adecuado
                            FechaProgramada = DateTime.Now.AddDays(7), // Programar para dentro de 7 días
                            Tipo = MantenimientoProgramado.TipoMantenimiento.Correctivo,
                            Descripcion = $"Mantenimiento correctivo programado por {tipo.NombreTipo}",
                            Estado = MantenimientoProgramado.EstadoMantenimiento.Pendiente
                            
                        }
                    );
                    break;
                case "KILOMETRAJE":
                    try
                    {
                        camion.KilometrajeActual = double.Parse(registroLog.Mensaje);
                    }
                    catch (Exception ex) {

                        return BadRequest("El mensaje del registro de log no es un número válido para el kilometraje actual del camión.");
                    }
                    Crud<Camion>.Update(camion.Codigo, camion);
                    var ultimoMantenimiento = await GetMantenimientoProgramado(camion.Codigo);

                    if(ultimoMantenimiento == null)
                    {
                        if(camion.KilometrajeActual >= 10000)
                        {
                            var taller1 = GetRandomTaller();
                            if (taller1 == null)
                            {
                                return NotFound("No hay talleres disponibles.");
                            }
                            Crud<MantenimientoProgramado>.Create(
                                  new MantenimientoProgramado
                                  {
                                    CamionCodigo = camion.Codigo,
                                    TallerCodigo = taller1.Codigo, // Asignar un taller por defecto o buscar uno adecuado
                                    FechaProgramada = DateTime.Now.AddDays(7), // Programar para dentro de 7 días
                                    Tipo = MantenimientoProgramado.TipoMantenimiento.Preventivo,
                                    Descripcion = $"Mantenimiento preventivo programado por {tipo.NombreTipo}",
                                    Estado = MantenimientoProgramado.EstadoMantenimiento.Pendiente

                                  }
                           );
                        }

                    }
                    else
                    {
                        if (camion.KilometrajeActual - ultimoMantenimiento.Kilometraje >= 10000)
                        {
                            var taller1 = GetRandomTaller();
                            if (taller1 == null)
                            {
                                return NotFound("No hay talleres disponibles.");
                            }
                            Crud<MantenimientoProgramado>.Create(
                                new MantenimientoProgramado
                                {
                                    CamionCodigo = camion.Codigo,
                                    TallerCodigo = taller1.Codigo, // Asignar un taller por defecto o buscar uno adecuado
                                    FechaProgramada = DateTime.Now.AddDays(7), // Programar para dentro de 7 días
                                    Tipo = MantenimientoProgramado.TipoMantenimiento.Preventivo,
                                    Descripcion = $"Mantenimiento preventivo programado por {tipo.NombreTipo}",
                                    Estado = MantenimientoProgramado.EstadoMantenimiento.Pendiente

                                }
                            );
                        }
                    }

                    break;
                
            }
            return CreatedAtAction(nameof(GetRegistroLog), new { codigo = registroLog.Codigo }, registroLog);
        }
        
       
        private async Task<Camion?> GetCamionByPlaca(string placa)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7253/api/Camiones/Get-By-Placa/" + placa);

                if (!response.IsSuccessStatusCode)
                {
                    return null;  // Si la respuesta no es exitosa, retornar null
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Usamos JObject para parsear el JSON manualmente
                var jsonObject = JObject.Parse(jsonResponse);

                // Mapeamos manualmente las propiedades del JSON al objeto Camion
                var camion = new Camion
                {
                    Codigo = (int)jsonObject["codigo"],
                    Modelo = (string)jsonObject["modelo"],
                    Anio = (int)jsonObject["anio"],
                    Placa = (string)jsonObject["placa"],
                    KilometrajeActual = (double)jsonObject["kilometrajeActual"],
                    Estado = (Estado)(int)jsonObject["estado"],  // Conversión del número al enum
                    Conductores = jsonObject["conductores"]?.ToObject<List<Conductor>>(),  // Si el campo es null, lo manejamos de forma segura
                    MantenimientosProgramados = jsonObject["mantenimientosProgramados"]?.ToObject<List<MantenimientoProgramado>>()
                };

                return camion;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        
        private async Task<MantenimientoProgramado?> GetMantenimientoProgramado(int codigoCamion)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7253/api/MantenimientosProgramadores/ultimo-mantenimiento/" + codigoCamion);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var mantenimiento = JsonSerializer.Deserialize<MantenimientoProgramado>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? null;

                return mantenimiento; // Se devuelve como JSON para manejarla en el cliente
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private  Taller? GetRandomTaller()
        {

            var talleres =  Crud<Taller>.GetAll();
            if (talleres == null || !talleres.Any())
            {
                return null; // No hay talleres disponibles
            }
            Random random = new Random();
            int index = random.Next(talleres.Count());
            return talleres[index]; // Retorna un taller aleatorio

        }
       
    }
}
