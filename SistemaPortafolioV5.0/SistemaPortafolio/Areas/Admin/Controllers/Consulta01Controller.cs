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
    public class Consulta01Controller : Controller
    {
        // GET: Admin/Consulta01
        Persona persona = new Persona();
        TipoPersona tpersona = new TipoPersona();
        public ActionResult Index()
        {
            ViewBag.tipo = persona.listartipo();
            return View();
        }
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(persona.ListarGrillac01(grid));
        }
    }
}