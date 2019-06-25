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
    public class TipoPersonaController : Controller
    {
        private TipoPersona tipopersona = new TipoPersona();
        // GET: Admin/TipoPersona
        public ActionResult Index()
        {
            
            //return View();
            return View(tipopersona.Listar());
        }
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(tipopersona.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(tipopersona.Obtener(id));
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            return View(
                id == 0 ? new TipoPersona()//generar un nuevo semestre
                : tipopersona.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(TipoPersona model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/tipopersona");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/tipopersona/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            tipopersona.tipopersona_id = id;
            tipopersona.Eliminar();
            return Redirect("~/tipopersona");
        }
        //consulta
        public ActionResult Consulta()
        {

            return View(tipopersona.Consulta());// devuelve la vista consulta
        }


    }
}