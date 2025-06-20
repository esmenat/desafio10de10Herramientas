using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema.Modelos; // Asegúrate de que el modelo 'Taller' esté disponible en esta ruta
using Gestion.API.Consumer;
using Sistema.Modelos.Modelos; // Asegúrate de que la clase Crud<Taller> esté correctamente configurada

namespace Sistema.MVC.Controllers
{
    public class TalleresController : Controller
    {
        // GET: TalleresController
        public ActionResult Index()
        {
            var data = Crud<Taller>.GetAll();
            return View(data);
        }

        // GET: TalleresController/Details/5
        public ActionResult Details(int id)
        {
            var taller = Crud<Taller>.GetById(id);
            return View(taller);
        }

        // GET: TalleresController/Create
        public ActionResult Create()
        {
            return View();
        }
       
        // POST: TalleresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Taller taller)
        {
            try
            {
                Crud<Taller>.Create(taller);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: TalleresController/Edit/5
        public ActionResult Edit(int id)
        {
            var taller = Crud<Taller>.GetById(id);
            return View(taller);
        }

        // POST: TalleresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Taller taller)
        {
            try
            {
                Crud<Taller>.Update(id, taller);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: TalleresController/Delete/5
        public ActionResult Delete(int id)
        {
            var taller = Crud<Taller>.GetById(id);
            return View(taller);
        }

        // POST: TalleresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Taller>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error al eliminar el taller. Asegúrese de que no esté asociado a otros registros.");
                return View();
            }
        }
    }
}
