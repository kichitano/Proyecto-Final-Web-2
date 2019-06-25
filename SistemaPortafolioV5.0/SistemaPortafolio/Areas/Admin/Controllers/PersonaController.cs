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
    public class PersonaController : Controller
    {
        // GET: Admin/Persona
        private Persona persona = new Persona();
        private Semestre semestre = new Semestre();
        private TipoPersona tipopersona = new TipoPersona();
        // GET: Persona
        public ActionResult Index(String criterio)
        {
            //return View(semestre.Listar());
            if (criterio == null || criterio == "")
            {
                return View(persona.Listar());
            }
            else
            {
                return View(persona.Buscar(criterio));
            }
        }
        //grilla
        public JsonResult CargarGrilla(AnexGRID grid)
        {
            return Json(persona.ListarGrilla(grid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ver(int id)
        {
            return View(persona.Obtener(id));
        }
        public ActionResult Buscar(string criterio)
        {
            return View(
                criterio == null || criterio == "" ? persona.Listar()/*? eso significa entonces*/
                : persona.Buscar(criterio)/*: eso significacaso contrario*/
                );
        }
        public ActionResult AgregarEditar(int id = 0)
        {
            ViewBag.tipo = persona.listartipo();   //para el combo
            ViewBag.semestre = semestre.Listar();   //Para autoseleccionar el semestre
            return View(
                id == 0 ? new Persona()//generar un nuevo semestre
                : persona.Obtener(id)//devuelve un registro por el id
                );
        }
        public ActionResult Guardar(Persona model,string SiDocente, string SiSemestre)
        {
            bool docente = false;
            foreach (var tp in tipopersona.Listar())
            {
                if((model.tipopersona_id == tp.tipopersona_id) && (tp.nombre.Equals("Docente")))
                {
                    docente = true;
                }
            }

            if (ModelState.IsValid)
            {
                model.Guardar(SiDocente, SiSemestre,docente);
                return Redirect("~/Admin/Persona");//se referencia al index automaticamente
            }
            else
            {
                return View("~/Admin/Persona/AgregarEditar.cshtml", model);
            }
        }
        public ActionResult Eliminar(int id)
        {
            persona.persona_id = id;
            persona.Eliminar();
            return Redirect("~/Admin/Persona");
        }
        //consulta
        public ActionResult Consulta()
        {

            return View(persona.Consulta());// devuelve la vista consulta
        }
        public ActionResult ConsultaBar()
        {

            return View(persona.Consulta());// devuelve la vista consulta
        }
    }
}