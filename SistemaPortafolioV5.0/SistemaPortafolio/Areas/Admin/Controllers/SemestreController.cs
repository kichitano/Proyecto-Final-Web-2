using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    [Autenticado]
    public class SemestreController : Controller
    {
        private Semestre semestre = new Semestre();
        // GET: Admin/Semestre
        public ActionResult Index(string criterio)
        {
            if (criterio == null || criterio == "")
            {
                return View(semestre.Listar());
            }
            else
            {
                return View(semestre.Buscar(criterio));
            }
        }

       

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(semestre.ListarGrilla(grid));
        }

        public ActionResult Ver(int id)
        {
            return View(semestre.Obtener(id));
        }

        public ActionResult Buscar(string criterio)
        {
            return View(
                criterio == null || criterio == "" ? semestre.Listar()
                : semestre.Buscar(criterio)
                );
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            return View(
                id == 0 ? new Semestre() //para generar un nuevo objeto
                : semestre.Obtener(id) // devuelve un id del semestre
                );
        }

        public ActionResult Guardar(Semestre model, string SemAnt)
        {
            if (ModelState.IsValid)
            {                
                model.Guardar(SemAnt);
                return Redirect("~/Admin/Semestre/Index/");
            }
            else
            {
                return View("~/Admin/Semestre/Index/AgregarEditar.cshtml", model);
            }
        }

        public ActionResult Eliminar(int id)
        {
            semestre.semestre_id = id;
            semestre.Eliminar();
            return Redirect("~/Admin/Semestre"); //devuelve la vista index
        }
    }
}