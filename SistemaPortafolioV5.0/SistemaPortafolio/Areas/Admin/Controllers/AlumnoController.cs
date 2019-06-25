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
    public class AlumnoController : Controller
    {
        Persona alumno = new Persona();
        // GET: Admin/Alumno
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(alumno.ListarGrillaA(grid));
        }

        public ActionResult Curso(int id)
        {
            Session["idd_persona"] = id;
            Curso curso = new Curso();
            ViewBag.cursoalumno = curso.cursoal(id);
            return View();
        }

        public JsonResult AgregarCurso(string[] codigo = null, string[] persona = null)
        {
            var rm = new ResponseModel();

            string codigoc = "";
            int personaa = 0;

            foreach(var c in codigo)
            {
                codigoc = c;
            }
            foreach(var p in persona)
            {
                personaa = Convert.ToInt32(p);
            }

            Session["idd_personaa"] = personaa.ToString();

            rm = alumno.agregarcurso(codigoc.Trim(), personaa);

            if (rm.response)
            {
                rm.href = Url.Content("/Admin/Alumno/Curso/" + personaa);
                return Json(rm);
            }
            else
            {
                return Json(rm);
            }
        }

        public ActionResult EliminarCurso(int id)
        {
            alumno.eliminarcurso(id);
            int idpersona = Convert.ToInt32(Session["idd_personaa"].ToString());
            return Redirect("~/Admin/Alumno/Curso/" + idpersona);
        }
    }
}