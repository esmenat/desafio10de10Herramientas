using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos; // Asegúrate de que el modelo 'MantenimientoProgramado' esté disponible en esta ruta
using Gestion.API.Consumer;
using Sistema.Modelos.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization; // Asegúrate de que la clase Crud<MantenimientoProgramado> esté correctamente configurada

namespace Sistema.MVC.Controllers
{
    [Authorize]
    public class MantenimientosProgramadosController : Controller
    {
        // GET: MantenimientosProgramadosController
        public ActionResult Index()
        {
            var data = Crud<MantenimientoProgramado>.GetAll();
            ViewBag.TotalRegistros = data.Count;
            return View(data);
        }

        // GET: MantenimientosProgramadosController/Details/5
        public ActionResult Details(int id)
        {
            var mantenimiento = Crud<MantenimientoProgramado>.GetById(id);

            return View(mantenimiento);
        }

        // GET: MantenimientosProgramadosController/Create
        public ActionResult Create()
        {
            ViewBag.Camiones = GetCamiones();
            ViewBag.Talleres = GetTalleres();
            return View();
        }
        private List<SelectListItem> GetCamiones()
        {
            var paises = Crud<Camion>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Codigo.ToString(),
                Text = p.Placa.ToString(),
            }).ToList();
        }
        private List<SelectListItem> GetTalleres()
        {
            var paises = Crud<Taller>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Codigo.ToString(),
                Text = p.Nombre,
            }).ToList();
        }
        // POST: MantenimientosProgramadosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MantenimientoProgramado mantenimiento)
        {
            try
            {
                Crud<MantenimientoProgramado>.Create(mantenimiento);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: MantenimientosProgramadosController/Edit/5
        public ActionResult Edit(int id)
        {
            var mantenimiento = Crud<MantenimientoProgramado>.GetById(id);
            ViewBag.Camiones = GetCamiones();
            ViewBag.Talleres = GetTalleres();

            return View(mantenimiento);
        }

        // POST: MantenimientosProgramadosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MantenimientoProgramado mantenimiento)
        {
            try
            {
                Crud<MantenimientoProgramado>.Update(id, mantenimiento);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: MantenimientosProgramadosController/Delete/5
        public ActionResult Delete(int id)
        {
            var mantenimiento = Crud<MantenimientoProgramado>.GetById(id);
            return View(mantenimiento);
        }

        // POST: MantenimientosProgramadosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<MantenimientoProgramado>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error al eliminar el mantenimiento programado. Asegúrese de que no esté asociado a otros registros.");
                return View();
            }
        }

        [HttpPost("realizar-mantenimiento")]
        [ValidateAntiForgeryToken]
        public ActionResult RealizarMantenimiento(int id)
        {
            try
            {
                // Obtén el mantenimiento programado por su id
                var mantenimiento = Crud<MantenimientoProgramado>.GetById(id);

                // Actualiza la fecha y el estado
                mantenimiento.FechaRealizada = DateTime.Now;
                mantenimiento.Estado = MantenimientoProgramado.EstadoMantenimiento.Finalizado;

                // Obtén el camión y actualiza el kilometraje
                var camion = Crud<Camion>.GetById(mantenimiento.CamionCodigo);
                mantenimiento.Kilometraje = camion.KilometrajeActual;

                // Guarda los cambios en la base de datos
                Crud<MantenimientoProgramado>.Update(id, mantenimiento);

                // Redirige a la vista "Index" después de completar el mantenimiento
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Si hay un error, muestra el mensaje de error
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }


    }
}
