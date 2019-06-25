using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;
using System.IO;

namespace SistemaPortafolio.Areas.User.Controllers
{
    [Autenticado]
    public class DocumentoController : Controller
    {

        Documento documento = new Documento();
        // GET: Admin/Documento
        public ActionResult Index()
        {
            Unidad unidad = new Unidad();
            ViewBag.semestre = unidad.listarsemestre();
            ViewBag.ciclo = documento.listarciclo();
            return View();
        }

        public JsonResult CargarGrilla(AnexGRID grid)
        {
            Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
            return Json(documento.ListarGrilla2(grid, usuario.Persona.persona_id));
        }

        public ActionResult Metadata(int id)
        {
            ViewBag.ultimametadata = documento.listarmetadatareciente(id);
            ViewBag.antiguametadata = documento.listarmetadataantigua(id);
            return View(documento.obtenerdocumento(id));
        }

        public ActionResult MiDocumento(string id,string buscar)
        {
            if(id != null || id != "")
            {
                Session["enlace"] = id;
            }

            string[] separar = Session["enlace"].ToString().Split('_');
            ViewBag.verificarpersona = documento.verificarpersona(separar[1]);
            ViewBag.documento = documento.listardocumentoo(separar[0], separar[1], buscar);
            ViewBag.docente = documento.listardocente(separar[0], separar[1]);
            ViewBag.tipodocumento = documento.listartipodocumento2(separar[1]);
            ViewBag.unidad = documento.listarunidad();
            return View();
        }

        public ActionResult IngresoDocumento(int id = 0)
        {
            if (id != 0)
            {
                Session["modificar"] = "si";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(documento.obtenerdocumento(id));
            }
            else
            {
                Session["modificar"] = "no";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(new Documento());
            }
        }

        public JsonResult SubirDocumento(Documento model, HttpPostedFileBase archivo, string[] aceptar = null)
        {
            bool flag = false;
            bool flagaceptar = false;

            if (Session["modificar"] == null || Session["modificar"].ToString().Equals("no"))
            {
                Documento doc = new Documento();
                var rm = new ResponseModel();
                doc = model;

                ModelState.Remove("documento_id");
                ModelState.Remove("archivo");
                ModelState.Remove("descripcion");
                ModelState.Remove("extension");
                ModelState.Remove("fecha_entrega");
                ModelState.Remove("hora_entrega");
                ModelState.Remove("tamanio");

                rm = model.GuardarArchivo(archivo);

                if (rm.response)
                {
                    rm.href = Url.Content("~/User/Documento");
                    return Json(rm);
                }
                else
                {
                    return Json(rm);
                }
            }
            else
            {
                if (model == null || archivo == null)
                {
                    model = (Documento)Session["help1"];
                    archivo = (HttpPostedFileBase)Session["help2"];
                }
                //verificar extension
                Documento ver = new Documento();

                var rmm = new ResponseModel();
                rmm = ver.verextension(archivo, model.tipodocumento_id);

                if (!rmm.response)
                {
                    return Json(rmm);
                }

                Documento tip = new Documento();
                string ti = tip.material(model.tipodocumento_id);
                Documento alumn = new Documento();
                bool doc_alumno = alumn.docalumno(model.tipodocumento_id); ;
                //validar la existencia de otro archivo del mismo tipo
                var rm = new ResponseModel();
                Unidad semestre = new Unidad();
                semestre = documento.obtenersemestre(model.id_unidad);

                string busqueda = "";
                Session["verificar"] = model;
                Session["verificararchivo"] = archivo;
                if (Session["verificar1"] != null)
                {
                    model = (Documento)Session["verificar1"];
                    archivo = (HttpPostedFileBase)Session["verificar1archivo"];
                    semestre = documento.obtenersemestre(model.id_unidad);
                }
               
                if (aceptar != null)
                {
                    foreach (var i in aceptar)
                    {
                        flagaceptar = true;
                    }
                }

                string nombrearchivo = "";
                busqueda = semestre.id_semestre + "_" + model.tipodocumento_id + "_" + model.id_unidad + "_" + model.curso_cod + "_" + model.persona_id + "_" + model.documento_id;
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Server/Docs/"));

                foreach (var fi in di.GetFiles())
                {
                    if (!fi.Name.Contains("~$"))
                    {
                        if (fi.Name.Contains(busqueda))
                        {
                            flag = true;
                            nombrearchivo = fi.FullName;
                            break;
                        }
                    }
                }

                if (flagaceptar)
                {
                    try
                    {
                        System.IO.File.Delete(nombrearchivo);
                        //documento.EliminarDocumentoRegistro(model);
                    }
                    catch (Exception e)
                    {

                    }
                    flag = false;
                }

                if (flag)
                {
                    rm.href = Url.Content("~/User/Documento/Verificar");
                    rm.SetResponse(true);
                    return Json(rm);
                }
                else
                {
                    Documento doc = new Documento();

                    doc = model;

                    rm = model.GuardarArchivo(archivo);

                    if (rm.response)
                    {
                        rm.href = Url.Content("~/User/Documento/Detalle/" + model.documento_id);
                        return Json(rm);
                    }
                    else
                    {
                        return Json(rm);
                    }
                }
            }
        }

        public ActionResult Detalle(int id)
        {
            Documento doc = new Documento();
            doc = documento.obtenerdocumento(id);

            ViewBag.ultimometadata = (MetadataDocumento)Session["antiguo"];
            ViewBag.metadatanuevo = (MetadataDocumento)Session["nuevo"];

            return View(doc);
        }

        public ActionResult Verificar()
        {
            Session["help1"] = (Documento)Session["verificar"];
            Session["help2"] = (HttpPostedFileBase)Session["verificararchivo"];
            Session["verificar1"] = (Documento)Session["verificar"];
            Session["verificar1archivo"] = (HttpPostedFileBase)Session["verificararchivo"];
            return View();
        }

        public FileResult DescargarDocumento(int id)
        {
            Documento doc = new Documento();
            doc = documento.obtenerdocumento(id);
            var ruta = Server.MapPath("~/Server/Docs/" + doc.archivo);

            string type = "";

            doc.extension = doc.extension.Trim();

            if(doc.extension.Equals("xlsx"))
            {
                type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            if (doc.extension.Equals("xls"))
            {
                type = "application/vnd.ms-excel";
            }
            if (doc.extension.Equals("docx"))
            {
                type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            if (doc.extension.Equals("doc"))
            {
                type = "application/msword";
            }
            if (doc.extension.Equals("pptx"))
            {
                type = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            }
            if (doc.extension.Equals("ppt"))
            {
                type = "application/vnd.ms-powerpoint";
            }

            return File(ruta, type, doc.descripcion);
        }

        public ActionResult EliminarDocumento(int id)
        {
            Documento doc = new Documento();
            doc = documento.obtenerdocumento(id);
            string codcurso = doc.curso_cod;
            doc.EliminarDocumento();

            var file = Path.Combine(HttpContext.Server.MapPath("/Server/Docs/"), doc.archivo);
            if (System.IO.File.Exists(file))
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception e)
                {

                }

            return Redirect("~/User/Documento/MiDocumento/" + Session["enlace"].ToString());
        }

        //prueba de entrada
        public ActionResult PruebaEntrada(int id = 0)
        {
            if(id != 0)
            {
                Session["modificar"] = "si";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento3(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(documento.obtenerdocumento(id));
            }
            else
            {
                Session["modificar"] = "no";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento3(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(new Documento());
            }
        }

        public ActionResult InformeFinalCurso(int id = 0)
        {
            if (id != 0)
            {
                Session["modificar"] = "si";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento3(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(documento.obtenerdocumento(id));
            }
            else
            {
                Session["modificar"] = "no";
                Usuario usuario = new Usuario().Obtener(SistemaPortafolio.Models.SessionHelper.GetUser());
                ViewBag.tipodocumento = documento.listartipodocumento3(usuario.Persona.TipoPersona.nombre);
                ViewBag.unidad = documento.listarunidadd();
                ViewBag.curso = documento.listarcurso(usuario.persona_id);
                return View(new Documento());
            }
        }
    }
}