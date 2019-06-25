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
    public class CursoDocenteController : Controller
    {
        // GET: Admin/CursoDocente
        public ActionResult Index()
        {
            return View();
        }
    }
}