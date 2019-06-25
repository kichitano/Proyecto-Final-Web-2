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
    public class UsuarioController : Controller
    {
        private Usuario usuario = new Usuario();
        private Persona persona = new Persona();
        private TipoUsuario tipousuario = new TipoUsuario();

        // GET: Admin/Usuario
        public ActionResult Index(String criterio)
        {
            return View();
            ////return View(semestre.Listar());
            //if (criterio == null || criterio == "")
            //{
            //    return View(usuario.Listar());
            //}
            //else
            //{
            //    return View(usuario.Buscar(criterio));
            //}
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(usuario.ListarGrilla(grid));
        }
        public ActionResult Ver(int id)
        {
            return View(usuario.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            return View(
                criterio == null || criterio == "" ? usuario.Listar()/*? eso significa entonces*/
                : usuario.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.Persona = persona.Listar();    //para el combo
            ViewBag.TipoUsuario = tipousuario.Listar();    //para el combo
            return View(
                id == 0 ? new Usuario()//generar un nuevo semestre
                : usuario.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Usuario model)
        {
            if (model.avatar == null || model.avatar == "")
            {
                model.avatar = "user_default.png";
            }
            if (model.clave == null || model.clave == "")
            {
                ModelState.Remove("clave");
            }
            model.Guardar();
            return Redirect("~/Admin/Usuario");
        }


        public ActionResult Eliminar(int id)
        {
            usuario.usuario_id = id;
            usuario.Eliminar();
            return Redirect("~/Admin/Usuario"); //devuelve la vista index
        }
        //consulta


    }
}