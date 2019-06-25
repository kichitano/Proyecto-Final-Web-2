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
    public class Consulta04Controller : Controller
    {
        private Notificacion notificacion = new Notificacion();
         // GET: Admin/Consulta04
        public ActionResult Index(string criterio)
        {
            if (criterio == null || criterio == " ")
            {

                return View();

            }
            else

            {
                return View(notificacion.Buscar(criterio));
            }

        }

        public ActionResult Buscar(string criterio)
        {


            return View(criterio == null || criterio == "" ? notificacion.Listar() : notificacion.Buscar(criterio));
        }
    }
}