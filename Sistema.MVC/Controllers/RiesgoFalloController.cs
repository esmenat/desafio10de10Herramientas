using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Sistema.Modelos.Modelos;

namespace Sistema.MVC.Controllers
{
    public class RiesgoFalloController : Controller
    {
        private readonly HttpClient _httpClient;

        public RiesgoFalloController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> CamionesProximosMantenimiento()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7253/api/MantenimientosProgramadores/alerta");

                if (!response.IsSuccessStatusCode)
                {
                    return Json(new List<MantenimientoProgramado>());  // Retornar lista vacía si la API no responde correctamente
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var mantenimientos = JsonSerializer.Deserialize<List<MantenimientoProgramado>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MantenimientoProgramado>();

                return Json(mantenimientos);  // Retornar los mantenimientos como JSON
            }
            catch (Exception ex)
            {
                // En caso de error, devolver una lista vacía
                return Json(new List<MantenimientoProgramado>());
            }
        }


    }
}