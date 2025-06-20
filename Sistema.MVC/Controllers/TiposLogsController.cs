using Gestion.API.Consumer;
using Logs.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema.Modelos.Modelos;

namespace Sistema.MVC.Controllers
{
    public class TiposLogsController : Controller
    {
        // GET: TiposLogsController
        public ActionResult Index()
        {
            var data = Crud<TipoLog>.GetAll();
            return View(data);
        }

        // GET: TiposLogsController/Details/5
        public ActionResult Details(int id)
        {
            var tipoLog = Crud<TipoLog>.GetById(id);
            return View(tipoLog);
        }

        // GET: TiposLogsController/Create
        public ActionResult Create()
        {
            return View();
        }
     

        // POST: TiposLogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoLog tipo)
        {
            try
            {
                Crud<TipoLog>.Create(tipo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TiposLogsController/Edit/5
        public ActionResult Edit(int id)
        {
            var tipoLog = Crud<TipoLog>.GetById(id);
            return View(tipoLog);
        }

        // POST: TiposLogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoLog tipoLog)
        {
            try
            {
                Crud<TipoLog>.Update(id,  tipoLog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TiposLogsController/Delete/5
        public ActionResult Delete(int id)
        {
            var tipoLog = Crud<TipoLog>.GetById(id);
            return View(tipoLog);
        }

        // POST: TiposLogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Crud<TipoLog>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
