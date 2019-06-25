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
    public class HojaVidaDocenteEXController : Controller
    {//
        private HojaVidaDocenteEX hojavidadocenteex = new HojaVidaDocenteEX();
        Usuario usuario = new Usuario();
        // GET: Persona
        public ActionResult Index(String criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            //return View(semestre.Listar());
            if (criterio == null || criterio == "")
            {
                return View(hojavidadocenteex.Listar(usuario.persona_id));
            }
            else
            {
                return View(hojavidadocenteex.Buscar(criterio));
            }
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(hojavidadocenteex.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(hojavidadocenteex.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            return View(
                criterio == null || criterio == "" ? hojavidadocenteex.Listar(usuario.persona_id)/*? eso significa entonces*/
                : hojavidadocenteex.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            return View(
                id == 0 ? new HojaVidaDocenteEX()//generar un nuevo semestre
                : hojavidadocenteex.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Persona model, string nada = "")
        {
            bool docente = false;

            if (ModelState.IsValid)
            {
                model.Guardar(nada,nada,docente);
                return Redirect("~/HojaVidaDocenteEX");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/HojaVidaDocenteEX/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            hojavidadocenteex.hojavidadocenteex_id = id;
            hojavidadocenteex.Eliminar();
            return Redirect("~/Admin/HojaVida/AgregarEditarEX");
        }

    }
}