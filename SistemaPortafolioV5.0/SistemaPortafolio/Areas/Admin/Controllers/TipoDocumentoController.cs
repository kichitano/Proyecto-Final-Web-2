using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaPortafolio.Areas.Admin.Controllers
{
    [Autenticado]
    public class TipoDocumentoController : Controller
    {
        TipoDocumento tipodocumento = new TipoDocumento();
        // GET: Admin/TipoDocumento
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.tipopersona = tipodocumento.listartipopersona();
            return View(
                id == 0 ? new TipoDocumento()
                : tipodocumento.Obtener(id)
                );
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(tipodocumento.ListarGrilla(grid));
        }

        public ActionResult Guardar(TipoDocumento model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/Admin/TipoDocumento/Index/");
            }
            else
            {
                return View("~/Admin/TipoDocumento/AgregarEditar/", model);
            }
        }

        public ActionResult Eliminar(int id)
        {
            tipodocumento.tipodocumento_id = id;
            tipodocumento.Eliminar();
            return Redirect("~/Admin/TipoDocumento");
        }
    }
}