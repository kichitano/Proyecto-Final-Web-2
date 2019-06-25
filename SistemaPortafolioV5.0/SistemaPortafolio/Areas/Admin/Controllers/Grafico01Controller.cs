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
    public class Grafico01Controller : Controller
    {
        private Usuario usuario = new Usuario();
        private TipoUsuario tipousuario = new TipoUsuario();
        // GET: Admin/Grafico01
        public ActionResult Index()
        {
            return View(usuario.ConsultaTotalUsuarios());
        }
      
    }
}