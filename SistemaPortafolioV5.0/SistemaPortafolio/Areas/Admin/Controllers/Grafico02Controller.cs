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
    public class Grafico02Controller : Controller
    {
        HojaVida hoja_vida = new HojaVida();
        // GET: Admin/Grafico02
        public ActionResult Index()
        {
            return View(hoja_vida.ConsultaTotalExperiencia());
        }
    }
}