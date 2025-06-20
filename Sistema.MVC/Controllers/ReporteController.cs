using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos.Modelos;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace Sistema.MVC.Controllers
{
    public class ReporteController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReporteController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7253/api/MantenimientosProgramadores/alerta");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new List<MantenimientoProgramado>());
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var mantenimientos = JsonSerializer.Deserialize<List<MantenimientoProgramado>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<MantenimientoProgramado>();

            // Filtrar los mantenimientos pendientes
            var mantenimientosPendientes = mantenimientos
                .Where(m => m.Estado == MantenimientoProgramado.EstadoMantenimiento.Pendiente)
                .ToList();

            // Separar en preventivos y correctivos
            var mantenimientosPreventivos = mantenimientosPendientes
                .Where(m => m.Tipo == MantenimientoProgramado.TipoMantenimiento.Preventivo)
                .ToList();

            var mantenimientosCorrectivos = mantenimientosPendientes
                .Where(m => m.Tipo == MantenimientoProgramado.TipoMantenimiento.Correctivo)
                .ToList();

            // Crear el reporte PDF
            var pdfStream = GeneratePdfReport(mantenimientosPreventivos, mantenimientosCorrectivos);

            // Devolver el PDF
            return File(pdfStream.ToArray(), "application/pdf", "ReporteMantenimientos.pdf");
        }

        private MemoryStream GeneratePdfReport(List<MantenimientoProgramado> mantenimientosPreventivos, List<MantenimientoProgramado> mantenimientosCorrectivos)
        {
            // El MemoryStream se mantiene abierto
            var memoryStream = new MemoryStream();

            // Crear un documento PDF
            var document = new PdfDocument();

            // Crear una página
            var page = document.AddPage();

            // Obtener el gráfico de la página
            var gfx = XGraphics.FromPdfPage(page);

            // Establecer la fuente
            var font = new XFont("Arial", 12);
            var titleFont = new XFont("Arial", 14, XFontStyle.Bold);

            // Márgenes
            int marginLeft = 40;
            int marginTop = 40;
            int yPosition = marginTop;

            // Título del reporte
            gfx.DrawString("Reporte de Mantenimientos Pendientes", titleFont, XBrushes.Black, marginLeft, yPosition);
            yPosition += 30;

            // Títulos para las listas
            gfx.DrawString("Mantenimientos Preventivos (Posibles falllos a futuro)", titleFont, XBrushes.Black, marginLeft, yPosition);
            yPosition += 20;

            // Dibujar las filas para los mantenimientos preventivos
            foreach (var mantenimiento in mantenimientosPreventivos)
            {
                gfx.DrawString($"Código: {mantenimiento.Codigo}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 15;
                gfx.DrawString($"Fecha Programada: {mantenimiento.FechaProgramada:dd/MM/yyyy}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 15;
                gfx.DrawString($"Descripción: {mantenimiento.Descripcion}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 25; // Espaciado entre registros
            }

            // Espacio entre listas
            yPosition += 20;

            // Títulos para las listas de correctivos
            gfx.DrawString("Mantenimientos Correctivos", titleFont, XBrushes.Black, marginLeft, yPosition);
            yPosition += 20;

            // Dibujar las filas para los mantenimientos correctivos
            foreach (var mantenimiento in mantenimientosCorrectivos)
            {
                gfx.DrawString($"Código: {mantenimiento.Codigo}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 15;
                gfx.DrawString($"Fecha Programada: {mantenimiento.FechaProgramada:dd/MM/yyyy}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 15;
                gfx.DrawString($"Descripción: {mantenimiento.Descripcion}", font, XBrushes.Black, marginLeft, yPosition);
                yPosition += 25; // Espaciado entre registros
            }

            // Guardar el documento en el MemoryStream
            document.Save(memoryStream);

            // No cerrar el MemoryStream, se deja abierto
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
    }
}
