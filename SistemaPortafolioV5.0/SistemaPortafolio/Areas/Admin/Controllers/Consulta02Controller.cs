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
    public class Consulta02Controller : Controller
    {
        private Curso curso = new Curso();
        Curso persona = new Curso();
        PlanEstudio plan_estudio = new PlanEstudio();

        // GET: Admin/Consulta02
        public ActionResult Index()
        {
            ViewBag.ciclo = curso.listarciclo();

            ViewBag.plan_estudio = plan_estudio.listar();

            return View();
        }
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(persona.ListarGrillac01(grid));
        }
    }
}