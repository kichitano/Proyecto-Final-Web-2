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
    public class UnidadController : Controller
    {
        Unidad unidad = new Unidad();
        // GET: Admin/Unidad
        public ActionResult Index()
        {
            ViewBag.semestre = unidad.listarsemestre();
            return View();
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.semestre = unidad.listarsemestre();
            return View(
                id == 0 ? new Unidad()
                : unidad.Obtener(id)
                );
        }

        public ActionResult Guardar(Unidad model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/Unidad/Index/");
            }
            else
            {
                return View("~/Admin/Unidad/AgregarEditar/", model);
            }
        }

        public ActionResult Eliminar(int id)
        {
            unidad.id_unidad = id;
            unidad.Eliminar();
            return Redirect("~/Admin/Unidad");
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(unidad.ListarGrilla(grid));
        }
    }
}