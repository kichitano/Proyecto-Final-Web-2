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
    public class NotificacionController : Controller
    {
        Notificacion notificacion = new Notificacion();
        // GET: Admin/Notificacion
        public ActionResult Index(int id)
        {
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
            return Json(notificacion.ListarGrilla(grid, usuario.persona_id));
        }

        public ActionResult MiNotificacion(int id)
        {
            notificacion.cambiarleido(id);
            return View(notificacion.obtener(id));
        }
    }
}