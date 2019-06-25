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
    public class CursoController : Controller
    {
        // GET: Admin/Curso
        private Curso curso = new Curso();
        public ActionResult Index(int id)
        {
            ViewBag.ciclo = curso.listarciclo();
            ViewBag.plan_id = id;
            Session["plan_id"] = id;
            return View();
        }

        public ActionResult AgregarEditar(string id)
        {
            ViewBag.ciclo = curso.listarciclo();
            ViewBag.plan = curso.obtenerplan(Convert.ToInt32(Session["plan_id"].ToString()));
            ViewBag.cursodocente = curso.listartododocente();
            ViewBag.cursodocen = curso.ObtenerCursoDocente(id);
            ViewBag.curso = curso.listarcurso(Convert.ToInt32(Session["plan_id"].ToString()));
            return View(
                id == "" || id == null ? new Curso()
                : curso.Obtener(id)
                );
        }

        public ActionResult Guardar(Curso model, int[] docente = null, string[] codigo_curso = null, string[] nuevo = null)
        {
            string g = "";
            if (nuevo != null)
            {
                foreach (var n in nuevo)
                {
                    g = n;
                }
            }

            if (g.Equals("si"))
            {
                if (codigo_curso != null)
                {
                    foreach (var d in codigo_curso)
                        model.curso_cod = d;
                }
                //para guardar en otra tabla que tiene relacion
                if (docente != null)
                {
                    foreach (var d in docente)
                        model.CursoDocente.Add(new CursoDocente { persona_id = d, curso_cod = model.curso_cod });
                }
                //fin

                if (ModelState.IsValid)
                {
                    model.GuardarNuevo();
                    return Redirect("~/Admin/Curso/Index/" + model.plan_id);
                }
                else
                {
                    return View("~/Admin/Curso/Index/AgregarEditar.cshtml", model);
                }
            }
            else
            {
                if (codigo_curso != null)
                {
                    foreach (var d in codigo_curso)
                        model.curso_cod = d;
                }
                //para guardar en otra tabla que tiene relacion
                if (docente != null)
                {
                    foreach (var d in docente)
                        model.CursoDocente.Add(new CursoDocente { persona_id = d, curso_cod = model.curso_cod });
                }
                //fin

                if (ModelState.IsValid)
                {
                    model.GuardarModificar();
                    return Redirect("~/Admin/Curso/Index/" + model.plan_id);
                }
                else
                {
                    return View("~/Admin/Curso/Index/AgregarEditar.cshtml", model);
                }
            }
        }

        public ActionResult Eliminar(string id)
        {
            curso.curso_cod = id;
            curso.Eliminar();
            return Redirect("~/Admin/Curso/Index/" + Session["plan_id"].ToString());
        }

        public JsonResult CargarGrilla(int id, AnexGRID grid)
        {
            return Json(curso.ListarGrilla(id, grid));
        }
        

        public ActionResult Ver(string id)
        {
            ViewBag.cursodocente = curso.listardocente(id);
            return View(curso.Obtener(id));
        }
    }
}