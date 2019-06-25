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
    public class Grafico05Controller : Controller
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
                    if (grafico.Equals("glinea"))
                    {
                        ViewBag.persona = person.ObtenerPersona(codigopersona);
                        ViewBag.unidad = documento.listarunidadactual();
                        Session["grafico"] = "glinea";
                        return View(documento.listardocumentooo(codigopersona));
                    }
                    else if (grafico.Equals("gbarraagrupada"))
                    {
                        ViewBag.persona = person.ObtenerPersona(codigopersona);
                        ViewBag.unidad = documento.listarunidadactual();
                        Session["grafico"] = "gbarraagrupada";
                        return View(documento.listardocumentooo(codigopersona));
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