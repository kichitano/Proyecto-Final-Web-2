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
    public class TipoUsuarioController : Controller
    {
        private TipoUsuario tipousuario = new TipoUsuario();
        // GET: Admin/TipoUsuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(tipousuario.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(tipousuario.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            return View(
                criterio == null || criterio == "" ? tipousuario.Listar()/*? eso significa entonces*/
                : tipousuario.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
         
            return View(
                id == 0 ? new TipoUsuario()//generar un nuevo semestre
                : tipousuario.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Usuario model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/tipousuario");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/tipousuario/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            tipousuario.tipousuario_id = id;
            tipousuario.Eliminar();
            return Redirect("~/tipousuario");
        }
        //consulta
       

    }
}