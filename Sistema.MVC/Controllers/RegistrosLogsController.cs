using Gestion.API.Consumer;
using Logs.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sistema.MVC.Controllers
{
    [Authorize]
    public class RegistrosLogsController : Controller
    {
        // GET: RegistrosLogsController
        public ActionResult Index(string searchTerm, string orderBy = "FechaHora", bool desc = false)
        {
            var data = Crud<RegistroLog>.GetAll(); // Obtiene todos los registros

            // Búsqueda insensible a mayúsculas/minúsculas
            if (!string.IsNullOrEmpty(searchTerm))
            {
                data = data.Where(r =>
                    r.Mensaje.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    r.PlacaCamion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Ordenar
            if (orderBy == "FechaHora")
            {
                data = desc ? data.OrderByDescending(r => r.FechaHora).ToList() : data.OrderBy(r => r.FechaHora).ToList();
            }
            else if (orderBy == "Codigo")
            {
                data = desc ? data.OrderByDescending(r => r.Codigo).ToList() : data.OrderBy(r => r.Codigo).ToList();
            }

            // Pasar los parámetros a la vista usando ViewData
            ViewData["SearchTerm"] = searchTerm;
            ViewData["OrderBy"] = orderBy;
            ViewData["Desc"] = desc;

            return View(data);
        }
    }
}
