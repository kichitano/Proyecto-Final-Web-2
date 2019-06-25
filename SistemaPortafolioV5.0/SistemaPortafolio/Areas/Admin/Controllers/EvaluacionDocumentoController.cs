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
    public class EvaluacionDocumentoController : Controller
    {
        Documento evaluar = new Documento();
        // GET: Admin/EvaluacionDocumento
        public ActionResult Index()
        {
            Unidad unidad = new Unidad();
            ViewBag.semestre = unidad.listarsemestre();
            ViewBag.ciclo = evaluar.listarciclo();
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(evaluar.ListarGrilla(grid));
        }

        public ActionResult Evaluar(string id,string buscar)
        {
            if (id != null || id != "")
            {
                Session["enlace"] = id;
            }
            string[] separar = Session["enlace"].ToString().Split('_');
            ViewBag.verificarpersona = evaluar.verificarpersona(separar[1]);
            ViewBag.documento = evaluar.listardocumentoo(separar[0], separar[1], buscar);
            ViewBag.docente = evaluar.listardocente(separar[0], separar[1]);
            ViewBag.tipodocumento = evaluar.listartipodocumento2(separar[1]);
            ViewBag.unidad = evaluar.listarunidad();
            return View();
        }

        public ActionResult Notificar(int id)
        {
            ViewBag.persona_receptor = evaluar.obtenerreceptor(id);
            return View(new Notificacion());
        }

        public ActionResult Notificacion(Notificacion model)
        {
            ModelState.Remove("fecha_emision");
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/EvaluacionDocumento/Index/");
            }
            else
            {
                return View("~/Admin/EvaluacionDocumento/Notificar", model);
            }
        }

        public ActionResult Estado(int id)
        {
            return View(evaluar.obtenerdocumento(id));
        }

        public ActionResult ModificarEstadoDocumento(Documento model, string[] curso_cod = null, string personaid = null)
        {
            foreach(var c in curso_cod)
            {
                model.curso_cod = c;
            }
            model.GuardarEstado();
            return Redirect("~/Admin/EvaluacionDocumento/Evaluar/" + model.curso_cod + "_" + personaid);
        }
    }
}