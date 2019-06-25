using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;

namespace SistemaPortafolio.Areas.User.Controllers
{
    [Autenticado]
    public class EvaluacionDocumentoController : Controller
    {
        Documento evaluar = new Documento();
        // GET: Admin/EvaluacionDocumento
        public ActionResult Index()
        {
            Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
            ViewBag.curso = evaluar.listarcursoo(usuario.persona_id);
            return View();
        }

        public JsonResult CargarGrillaA(AnexGRID grid)
        {
            Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
            return Json(evaluar.ListarGrillaA(grid, usuario.persona_id));
        }

        public ActionResult Evaluar(string id)
        {
            string[] separar = id.Split('_');
            ViewBag.documento = evaluar.listardocumentoa(separar[0],separar[1]);
            ViewBag.docente = evaluar.listardocentea(separar[0], separar[1]);
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
                return Redirect("~/User/EvaluacionDocumento/Index/");
            }
            else
            {
                return View("~/User/EvaluacionDocumento/Notificar", model);
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
            return Redirect("~/User/EvaluacionDocumento/Evaluar/" + model.curso_cod + "_" + personaid);
        }
    }
}