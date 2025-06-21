using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos; // Asegúrate de que el modelo 'Licencia' esté disponible en esta ruta
using Gestion.API.Consumer;
using Sistema.Modelos.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization; // Asegúrate de que la clase Crud<Licencia> esté correctamente configurada

namespace Sistema.MVC.Controllers
{
    [Authorize]
    public class LicenciasController : Controller
    {
        // GET: LicenciasController
        public ActionResult Index()
        {
            var data = Crud<Licencia>.GetAll();
            return View(data);
        }

        // GET: LicenciasController/Details/5
        public ActionResult Details(int id)
        {
            var licencia = Crud<Licencia>.GetById(id);
            return View(licencia);
        }

        // GET: LicenciasController/Create
        public ActionResult Create()
        {
            ViewBag.Conductores = GetConductores();
            return View();
        }
        private List<SelectListItem> GetConductores()
        {
            var paises = Crud<Conductor>.GetAll();
            return paises.Select(p => new SelectListItem
            {
                Value = p.Codigo.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        // POST: LicenciasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Licencia licencia)
        {
            try
            {
                Crud<Licencia>.Create(licencia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: LicenciasController/Edit/5
        public ActionResult Edit(int id)
        {
            var licencia = Crud<Licencia>.GetById(id);
            ViewBag.Conductores = GetConductores();
            return View(licencia);
        }

        // POST: LicenciasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Licencia licencia)
        {
            try
            {
                Crud<Licencia>.Update(id, licencia);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: LicenciasController/Delete/5
        public ActionResult Delete(int id)
        {
            var licencia = Crud<Licencia>.GetById(id);
            return View(licencia);
        }

        // POST: LicenciasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Licencia>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error al eliminar la licencia. Asegúrese de que no esté asociada a otros registros.");
                return View();
            }
        }
    }
}
