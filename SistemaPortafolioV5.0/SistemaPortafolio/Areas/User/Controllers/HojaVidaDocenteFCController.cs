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
    public class HojaVidaDocenteFCController : Controller
    {//
        
        private HojaVidaDocenteFC hojavidadocentefc = new HojaVidaDocenteFC();
        Usuario usuario = new Usuario();
        // GET: Persona
        // GET: Admin/HojaVidaDocenteFC
        public ActionResult Index(string criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            //return View(semestre.Listar());
            if (criterio == null || criterio == "")
            {
                return View(hojavidadocentefc.Listar(usuario.persona_id));
            }
            else
            {
                return View(hojavidadocentefc.Buscar(criterio));
            }
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(hojavidadocentefc.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(hojavidadocentefc.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            return View(
                criterio == null || criterio == "" ? hojavidadocentefc.Listar(usuario.persona_id)/*? eso significa entonces*/
                : hojavidadocentefc.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            return View(
                id == 0 ? new HojaVidaDocenteFC()//generar un nuevo semestre
                : hojavidadocentefc.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Persona model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar(null,null,false);
                return Redirect("~/HojaVidaDocenteFC");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/HojaVidaDocenteFC/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            hojavidadocentefc.hojavidadocentefc_id = id;
            hojavidadocentefc.Eliminar();
            return Redirect("~/User/HojaVida/AgregarEditarFC");
        }
    }
}