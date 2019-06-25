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
    public class HojaVidaDocenteFAController : Controller
    {//
        
        private HojaVidaDocenteFA hojavidadocentefa = new HojaVidaDocenteFA();
        Usuario usuario = new Usuario();

        // GET: Admin/HojaVidaDocenteFA
        // GET: Persona
        public ActionResult Index(string criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            //return View(semestre.Listar());
            if (criterio == null || criterio == "")
            {
                return View(hojavidadocentefa.Listar(usuario.persona_id));
            }
            else
            {
                return View(hojavidadocentefa.Buscar(criterio));
            }
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(hojavidadocentefa.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(hojavidadocentefa.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            usuario.Obtener(SessionHelper.GetUser());
            return View(
                criterio == null || criterio == "" ? hojavidadocentefa.Listar(usuario.persona_id)/*? eso significa entonces*/
                : hojavidadocentefa.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            return View(
                id == 0 ? new HojaVidaDocenteFA()//generar un nuevo semestre
                : hojavidadocentefa.Obtener(id)//devuelve un registro por el id
                );
        }

        public ActionResult Guardar(Persona model, string nada= "")
        {
            if (ModelState.IsValid)
            {
                model.Guardar(nada,nada,false);
                return Redirect("~/HojaVidaDocenteFA");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/HojaVidaDocenteFA/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            hojavidadocentefa.hojavidadocentefa_id = id;
            hojavidadocentefa.Eliminar();
            return Redirect("~/User/HojaVida/AgregarEditarFA");
        }
    }
}