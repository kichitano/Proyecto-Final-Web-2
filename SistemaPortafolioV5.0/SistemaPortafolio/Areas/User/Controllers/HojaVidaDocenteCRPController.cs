using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;

namespace SistemaPortafolio.Areas.User.Controllers
{
    [Autenticado]
    public class HojaVidaDocenteCRPController : Controller
    {//
        private HojaVida hojavida = new HojaVida();
        private HojaVidaDocenteCRP crp = new HojaVidaDocenteCRP();
        Usuario usuario = new Usuario();
        // GET: Usuario
        public ActionResult Index()
        {
            usuario.Obtener(SessionHelper.GetUser());
            return View(crp.Listar(usuario.persona_id));
          
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(crp.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(crp.Obtener(id));
        }
      
        public ActionResult AgregarEditar(int id = 0)
        {
            usuario.Obtener(SessionHelper.GetUser());
            ViewBag.Persona = crp.Listar(usuario.persona_id);    //para el combo
            ViewBag.Rango = hojavida.Listar();    //para el combo
            return View(
                id == 0 ? new HojaVidaDocenteCRP()//generar un nuevo semestre
                : crp.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Usuario model)
        {
            if (ModelState.IsValid)
            {
                model.Guardar();
                return Redirect("~/HojaVida");//se referencia al index automaticamente
            }
            else
            {
                return View("~/views/HojaVida/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            crp.hojavidadocentecrp_id = id;
            crp.Eliminar();
            return Redirect("~/User/HojaVida/AgregarEditarCRP");
        }
        //consulta
    


    }
}