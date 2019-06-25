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
    public class ConfiguracionEntregaController : Controller
    {
        ConfigEntrega configuracion = new ConfigEntrega();
        // GET: Admin/ConfiguracionEntrega
        public ActionResult Index()
        {
            Unidad unidad = new Unidad();
            ViewBag.unidad = configuracion.listarunidad();
            ViewBag.semestre = unidad.listarsemestre();
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(configuracion.ListarGrilla(grid));
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.unidad = configuracion.listarunidad();
            return View(
                id == 0 ? new ConfigEntrega()
                : configuracion.Obtener(id)
                );
        }

        public ActionResult Guardar(ConfigEntrega model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/ConfiguracionEntrega/Index/");
            }
            else
            {
                return View("~/Admin/ConfiguracionEntrega/AgregarEditar/", model);
            }
        }

        public ActionResult Ver(int id)
        {
            return View(configuracion.Obtener(id));
        }

        public ActionResult Eliminar(int id)
        {
            configuracion.configentrega_id = id;
            configuracion.Eliminar();
            return Redirect("~/Admin/ConfiguracionEntrega");
        }
    }
}