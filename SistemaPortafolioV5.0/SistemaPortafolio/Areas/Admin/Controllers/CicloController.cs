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
    public class CicloController : Controller
    {
        private Ciclo ciclo = new Ciclo();
        // GET: Admin/Ciclo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            return View(id == 0 ? new Ciclo()//generar un nuevo semestre
                     : ciclo.Obtener(id));//devuelve un registro por el ID);

        }

        public ActionResult Guardar(Ciclo model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/Ciclo");//devuelve por default Index
            }
            else
            {
                return View("~/views/Admin/Ciclo/AgregarEditar.cshtml", model);
            }

        }

        public ActionResult Eliminar(int id)
        {
            ciclo.ciclo_id = id;
            ciclo.Eliminar();
            return Redirect("~/Admin/Ciclo");
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(ciclo.ListarGrilla(grid));
        }

        public ActionResult Ver(int id)
        {
            return View(ciclo.Obtener(id));
        }
    }
}