using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos; // Asegúrate de que el modelo 'Conductor' esté disponible en esta ruta
using Gestion.API.Consumer;
using Sistema.Modelos.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering; // Asegúrate de que la clase Crud<Conductor> esté correctamente configurada

namespace Sistema.MVC.Controllers
{
    public class ConductoresController : Controller
    {
        // GET: ConductoresController
        public ActionResult Index()
        {
            var data = Crud<Conductor>.GetAll();
           
            return View(data);
        }

        // GET: ConductoresController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Conductor>.GetById(id);
            return View(data);
        }

        // GET: ConductoresController/Create
        public ActionResult Create()
        {
          
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

        // POST: ConductoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Conductor conductor)
        {
            try
            {
                Crud<Conductor>.Create(conductor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: ConductoresController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Conductor>.GetById(id);
          
            return View(data);
        }

        // POST: ConductoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Conductor conductor)
        {
            try
            {
                Crud<Conductor>.Update(id, conductor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: ConductoresController/Delete/5
        public ActionResult Delete(int id)
        {
            var conductor = Crud<Conductor>.GetById(id);
            return View(conductor);
        }

        // POST: ConductoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Conductor>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error al eliminar el conductor. Asegúrese de que no esté asociado a otros registros.");
                return View();
            }
        }
    }
}
