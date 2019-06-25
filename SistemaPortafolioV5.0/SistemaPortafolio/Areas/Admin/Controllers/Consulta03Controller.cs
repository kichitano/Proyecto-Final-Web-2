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
    public class Consulta03Controller : Controller
    {
        Persona persona = new Persona();

        // GET: Admin/Consulta03
        public ActionResult Index(string codigopersona)
        {
            if(codigopersona == null)
            {
                codigopersona = "0";
            }
            codigopersona = codigopersona.Trim();
            ViewBag.persona = persona.ObtenerPersona(codigopersona);
            Session["codigopersona"] = codigopersona;
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            if(!Session["codigopersona"].ToString().Equals("0"))
            {
                return Json(persona.ListarGrillaCCHVD(grid, Session["codigopersona"].ToString()));

            }
            else
            {
                return Json(grid);
            }
        }
    }
}