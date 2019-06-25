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
    public class Consulta05Controller : Controller
    {
        MetadataDocumento meta = new MetadataDocumento();
        // GET: Admin/Consulta05
        public ActionResult Index(string semestre, string tipodocumento, string codigopersona)
        {
            if (semestre != "" && tipodocumento != "" && codigopersona != "")
            {
                Session["tipodoc"] = meta.obtenertipodocumento(tipodocumento);
                ViewBag.tipodocumento = meta.listartipodocumento();
                ViewBag.semestre = meta.listarsemestre();
                ViewBag.ultimametadata = meta.listarmetadatareciente(semestre,tipodocumento,codigopersona);
                ViewBag.antiguametadata = meta.listarmetadataantigua(semestre, tipodocumento, codigopersona);
                return View();
            }
            else
            {
                Session["tipodoc"] = meta.obtenertipodocumento(tipodocumento);
                ViewBag.tipodocumento = meta.listartipodocumento();
                ViewBag.semestre = meta.listarsemestre();
                ViewBag.ultimametadata = meta.listarmetadatareciente(semestre, tipodocumento, codigopersona);
                ViewBag.antiguametadata = meta.listarmetadataantigua(semestre, tipodocumento, codigopersona);
                return View();
            }
        }
    }
}