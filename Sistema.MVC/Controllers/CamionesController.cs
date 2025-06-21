using Gestion.API.Consumer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Modelos;
using Sistema.Modelos.Modelos;

namespace Sistema.MVC.Controllers
{
    [Authorize]
    public class CamionesController : Controller
    {
        // GET: CamionesController
        public ActionResult Index()
        {
            var data = Crud<Camion>.GetAll();

            return View(data);
        }

        // GET: CamionesController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud<Camion>.GetById(id);
            return View(data);
        }

        // GET: CamionesController/Create
        public ActionResult Create()
        { 
            return View();
        }
        
        // POST: CamionesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Camion camion)
        {
            try
            {
                Crud<Camion>.Create(camion);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: CamionesController/Edit/5
        public ActionResult Edit(int id)
        {
            var camion = Crud<Camion>.GetById(id);
            return View(camion);
        }

        // POST: CamionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Camion camion)
        {
            try
            {
                Crud<Camion>.Update(id,camion);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: CamionesController/Delete/5
        public ActionResult Delete(int id)
        {
            var camion = Crud<Camion>.GetById(id);
            return View(camion);
        }

        // POST: CamionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<Camion>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Error al eliminar el camión. Asegúrese de que no esté asociado a otros registros.");
                return View();
            }
        }
    }
}
