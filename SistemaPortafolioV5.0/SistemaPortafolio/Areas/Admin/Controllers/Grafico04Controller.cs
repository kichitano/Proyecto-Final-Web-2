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
    public class Grafico04Controller : Controller
    {
        Documento documento = new Documento();
        Persona person = new Persona();
        public ActionResult Index(string grafico, string codigopersona)
        {
            try
            {
                Session["grafico"] = "";
                if (grafico.Equals(""))
                {
                    return View();
                }
                else
                {
                    if (grafico.Equals("gbarra"))
                    {
                        ViewBag.persona = person.ObtenerPersona(codigopersona);
                        ViewBag.semestre = documento.listarsemestre();
                        ViewBag.metadata = documento.listarmetadata(codigopersona);
                        ViewBag.tipodocumento = documento.listartipodocumentoo(codigopersona);
                        Session["grafico"] = "gbarra";
                        return View(documento.listartipodocumentoo(codigopersona));
                    }
                    else if (grafico.Equals("garea"))
                    {
                        ViewBag.persona = person.ObtenerPersona(codigopersona);
                        ViewBag.semestre = documento.listarsemestre();
                        ViewBag.metadata = documento.listarmetadata(codigopersona);
                        ViewBag.tipodocumento = documento.listartipodocumentoo(codigopersona);
                        Session["grafico"] = "garea";
                        return View(documento.listartipodocumentoo(codigopersona));
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}