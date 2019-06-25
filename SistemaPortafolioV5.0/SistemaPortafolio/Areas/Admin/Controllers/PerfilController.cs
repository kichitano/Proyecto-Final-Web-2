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
    public class PerfilController : Controller
    {
        private Usuario usuario = new Usuario();
        // GET: Perfil
        public ActionResult Index()
        {
            return View(usuario.Obtener(SessionHelper.GetUser())); //le vamos a pasar el id del usuario que esta logueado
        }

        public JsonResult Actualizar(Usuario model, HttpPostedFileBase Foto)
        {
            var rm = new ResponseModel();

            //retirar para que no se actualicen
            ModelState.Remove("usuario_id");
            ModelState.Remove("persona_id");
            ModelState.Remove("tipousuario");
            ModelState.Remove("estado");


            rm = model.GuardarPerfil(Foto);
            rm.href = Url.Content("/Admin/Home/Index");
            if(model.clave != null)
            {
                Logout();
                return Json(rm);
            }
            else
            {
                return Json(rm);
            }
        }

        public ActionResult Logout()
        {
            SessionHelper.DestroyUserSession();

            return Redirect("~/Login");
        }
    }
}