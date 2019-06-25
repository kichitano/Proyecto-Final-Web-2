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
    public class PlanEstudioController : Controller
    {
        private PlanEstudio planestudio = new PlanEstudio();
        // GET: Admin/PlanEstudio
        public ActionResult Index()
        {
            ViewBag.semestre = planestudio.listarsemestre();
            return View();
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.semestre = planestudio.listarsemestre();
            return View(
                id == 0 ? new PlanEstudio()
                : planestudio.Obtener(id)
                );
        }

        public ActionResult Guardar(PlanEstudio model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/PlanEstudio");
            }
            else
            {
                return View("~/Admin/Views/PlanEstudio/AgregarEditar.cshtml", model);
            }
        }

        public ActionResult Eliminar(int id)
        {
            planestudio.plan_id = id;
            planestudio.Eliminar();
            return Redirect("~/Admin/PlanEstudio");
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(planestudio.ListarGrilla(grid));
        }

        public ActionResult Ver(int id)
        {
            ViewBag.curso = planestudio.listarcursoplan(id);
            return View(planestudio.Obtener(id));
        }
    }
}
    