namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Data.Entity.Validation;
    using System.Web;
    using System.IO;
    using System.Collections;

    [Table("Documento")]
    public partial class Documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documento()
        {
            MetadataDocumento = new HashSet<MetadataDocumento>();
        }

        [Key]
        public int documento_id { get; set; }

        public int tipodocumento_id { get; set; }

        public int persona_id { get; set; }

        public int id_unidad { get; set; }

        [Required]
        [StringLength(100)]
        public string curso_cod { get; set; }

        [Required]
        [StringLength(150)]
        public string archivo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string descripcion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_entrega { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime hora_entrega { get; set; }

        [StringLength(10)]
        public string extension { get; set; }

        [StringLength(50)]
        public string tamanio { get; set; }

        [Required]
        [StringLength(50)]
        public string estado { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Unidad Unidad { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetadataDocumento> MetadataDocumento { get; set; }

        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    //incluir varias tablas de otras tablas, codigo en la siguiente linea
                    var query = db.CursoDocente.Include("Curso").Include(x => x.Curso.PlanEstudio.Semestre).Include(x => x.Curso.Ciclo).Include("Persona").Where(x => x.cursodocente_id > 0 && x.Persona.TipoPersona.nombre.Equals("Docente"));

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Persona.persona_id)
                                                               : query.OrderBy(x => x.Persona.persona_id);
                    }

                    if (grilla.columna == "semestre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.PlanEstudio.Semestre.semestre_id)
                                                               : query.OrderBy(x => x.Curso.PlanEstudio.Semestre.semestre_id);
                    }

                    if (grilla.columna == "ciclo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.Ciclo.descripcion)
                                                               : query.OrderBy(x => x.Curso.Ciclo.descripcion);
                    }

                    if (grilla.columna == "nombrecurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.nombre)
                                                               : query.OrderBy(x => x.Curso.nombre);
                    }

                    if (grilla.columna == "docente")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => (x.Persona.nombre + " " + x.Persona.apellido))
                                                               : query.OrderBy(x => (x.Persona.nombre + " " + x.Persona.apellido));
                    }

                    if (grilla.columna == "codigocurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.curso_cod)
                                                               : query.OrderBy(x => x.Curso.curso_cod);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "ciclo" && f.valor != "")
                            query = query.Where(x => x.Curso.Ciclo.ciclo_id.ToString().Contains(f.valor));

                        if (f.columna == "nombrecurso" && f.valor != "")
                            query = query.Where(x => x.Curso.nombre.Contains(f.valor));

                        if (f.columna == "docente" && f.valor != "")
                            query = query.Where(x => (x.Persona.nombre + " " + x.Persona.apellido).Contains(f.valor));

                        if (f.columna == "semestre" && f.valor != "")
                            query = query.Where(x => x.Curso.PlanEstudio.Semestre.semestre_id.ToString().Contains(f.valor));
                    }


                    var persona = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in persona
                                   select new
                                   {
                                       codigo = s.Persona.persona_id,
                                       semestre = s.Curso.PlanEstudio.Semestre.nombre,
                                       ciclo = s.Curso.Ciclo.descripcion,
                                       nombrecurso = s.Curso.nombre,
                                       docente = s.Persona.nombre + " " + s.Persona.apellido,
                                       codigocurso = s.Curso.curso_cod,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public AnexGRIDResponde ListarGrillaA(AnexGRID grilla,int personaid)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    CursoDocente curso = new CursoDocente();
                    curso = db.CursoDocente.Where(x => x.persona_id == personaid).SingleOrDefault();

                    //incluir varias tablas de otras tablas, codigo en la siguiente linea
                    var query = db.CursoDocente.Include("Curso").Include(x => x.Curso.PlanEstudio.Semestre).Include(x => x.Curso.Ciclo).Include("Persona").Include(x => x.Persona.TipoPersona).Where(x => x.cursodocente_id > 0 && x.Persona.TipoPersona.nombre.Equals("Alumno") && x.curso_cod.Equals(curso.curso_cod));

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Persona.persona_id)
                                                               : query.OrderBy(x => x.Persona.persona_id);
                    }

                    if (grilla.columna == "semestre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.PlanEstudio.Semestre.semestre_id)
                                                               : query.OrderBy(x => x.Curso.PlanEstudio.Semestre.semestre_id);
                    }

                    if (grilla.columna == "ciclo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.Ciclo.descripcion)
                                                               : query.OrderBy(x => x.Curso.Ciclo.descripcion);
                    }

                    if (grilla.columna == "nombrecurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.nombre)
                                                               : query.OrderBy(x => x.Curso.nombre);
                    }

                    if (grilla.columna == "docente")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => (x.Persona.nombre + " " + x.Persona.apellido))
                                                               : query.OrderBy(x => (x.Persona.nombre + " " + x.Persona.apellido));
                    }

                    if (grilla.columna == "codigocurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.curso_cod)
                                                               : query.OrderBy(x => x.Curso.curso_cod);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "curso_cod" && f.valor != "")
                            query = query.Where(x => x.curso_cod.Contains(f.valor));
                    }


                    var persona = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in persona
                                   select new
                                   {
                                       codigo = s.Persona.persona_id,
                                       semestre = s.Curso.PlanEstudio.Semestre.nombre,
                                       ciclo = s.Curso.Ciclo.descripcion,
                                       nombrecurso = s.Curso.nombre,
                                       alumno = s.Persona.nombre + " " + s.Persona.apellido,
                                       codigocurso = s.Curso.curso_cod,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public AnexGRIDResponde ListarGrilla2(AnexGRID grilla, int persona_id)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    //incluir varias tablas de otras tablas, codigo en la siguiente linea
                    var query = db.CursoDocente.Include("Curso").Include(x => x.Curso.PlanEstudio.Semestre).Include(x => x.Curso.Ciclo).Include("Persona").Where(x => x.cursodocente_id > 0).Where(x => x.Persona.persona_id == persona_id);

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Persona.persona_id)
                                                               : query.OrderBy(x => x.Persona.persona_id);
                    }

                    if (grilla.columna == "semestre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.PlanEstudio.Semestre.semestre_id)
                                                               : query.OrderBy(x => x.Curso.PlanEstudio.Semestre.semestre_id);
                    }

                    if (grilla.columna == "ciclo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.Ciclo.descripcion)
                                                               : query.OrderBy(x => x.Curso.Ciclo.descripcion);
                    }

                    if (grilla.columna == "nombrecurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.nombre)
                                                               : query.OrderBy(x => x.Curso.nombre);
                    }

                    if (grilla.columna == "codigocurso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Curso.curso_cod)
                                                               : query.OrderBy(x => x.Curso.curso_cod);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "ciclo" && f.valor != "")
                            query = query.Where(x => x.Curso.Ciclo.ciclo_id.ToString().Contains(f.valor));

                        if (f.columna == "nombrecurso" && f.valor != "")
                            query = query.Where(x => x.Curso.nombre.Contains(f.valor));

                        if (f.columna == "semestre" && f.valor != "")
                            query = query.Where(x => x.Curso.PlanEstudio.Semestre.semestre_id.ToString().Contains(f.valor));
                    }


                    var persona = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in persona
                                   select new
                                   {
                                       codigo = s.Persona.persona_id,
                                       semestre = s.Curso.PlanEstudio.Semestre.nombre,
                                       ciclo = s.Curso.Ciclo.descripcion,
                                       nombrecurso = s.Curso.nombre,
                                       codigocurso = s.Curso.curso_cod,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public List<Ciclo> listarciclo()
        {
            var ciclo = new List<Ciclo>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    ciclo = db.Ciclo.ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return ciclo;
        }

        public List<CursoDocente> listarcursoo(int persona)
        {
            var curso = new List<CursoDocente>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.CursoDocente.Include("Curso").Where(x => x.persona_id == persona).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso;
        }

        public List<Unidad> listarunidad()
        {
            var unidad = new List<Unidad>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    unidad = db.Unidad.ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return unidad;
        }

        public List<Documento> listardocumento(string cod_cur,string persona)
        {
            var documento = new List<Documento>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    documento = db.Documento.Include("Curso").Include("TipoDocumento").Include("Persona").Include("Unidad").Include(x => x.Persona.TipoPersona).Where(x => x.curso_cod.Contains(cod_cur) && x.persona_id.ToString().Equals(persona)).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return documento;
        }

        public List<Documento> listardocumentoo(string cod_cur, string persona, string buscar)
        {
            var documento = new List<Documento>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    if(buscar == null || buscar == "")
                    {
                        return documento;
                    }
                    long number1 = 0;
                    bool canConvert = long.TryParse(buscar, out number1);
                    if (canConvert)
                    {
                        documento = db.Documento.Include("Curso").Include("TipoDocumento").Include("Persona").Include("Unidad").Include(x => x.Persona.TipoPersona).Where(x => x.curso_cod.Contains(cod_cur) && x.persona_id.ToString().Equals(persona)).Where(x => x.tipodocumento_id.ToString().Equals(buscar)).ToList();

                        TipoDocumento doc = new TipoDocumento();
                        doc = db.TipoDocumento.Where(x => x.tipodocumento_id.ToString().Equals(buscar)).SingleOrDefault();

                        string bus = doc.nombre;

                        foreach (var i in documento)
                        {
                            bus = i.TipoDocumento.nombre;
                            break;
                        }
                        HttpContext.Current.Session["buscar"] = bus;
                    }
                    else
                    {
                        documento = db.Documento.Include("Curso").Include("TipoDocumento").Include("Persona").Include("Unidad").Include(x => x.Persona.TipoPersona).Where(x => x.curso_cod.Contains(cod_cur) && x.persona_id.ToString().Equals(persona)).Where(x => x.TipoDocumento.nombre.ToLower() != "informe prueba de entrada" && x.TipoDocumento.nombre.ToLower() != "informe final del curso").ToList();
                        HttpContext.Current.Session["buscar"] = "informe portafolio del curso";
                    }

                }
            }
            catch (Exception e)
            {
                throw;
            }

            return documento;
        }

        public List<TipoDocumento> listartipodocumento2(string tipopersona)
        {
            var documento = new List<TipoDocumento>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    Persona persona = new Persona();
                    persona = db.Persona.Include("TipoPersona").Where(x => x.persona_id.ToString().Equals(tipopersona)).SingleOrDefault();
                    documento = db.TipoDocumento.Include(x => x.TipoPersona).Where(x => x.tipopersona_id == persona.tipopersona_id).ToList(); 
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return documento;
        }

        public Persona verificarpersona(string personaa)
        {
            var persona = new Persona();

            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona.Include("TipoPersona").Where(x => x.persona_id.ToString().Equals(personaa)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return persona;
        }

        public List<Documento> listardocumentoa(string cod_cur, string personaid)
        {
            var documento = new List<Documento>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    documento = db.Documento.Include("Curso").Include("TipoDocumento").Include("Persona").Include("Unidad").Where(x => x.curso_cod.Contains(cod_cur) && x.persona_id.ToString().Equals(personaid)).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return documento;
        }

        public CursoDocente listardocente(string cod_cur, string persona)
        {
            var docente = new CursoDocente();

            try
            {
                using (var db = new ModeloDatos())
                {
                    docente = db.CursoDocente.Include("Curso").Include(x => x.Curso.Ciclo).Include("Persona").Include(x => x.Persona.TipoPersona).Where(x => x.curso_cod.Contains(cod_cur) && x.persona_id.ToString().Equals(persona)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return docente;
        }

        public CursoDocente listardocentea(string cod_cur, string personaid)
        {
            var docente = new CursoDocente();

            try
            {
                using (var db = new ModeloDatos())
                {
                    docente = db.CursoDocente.Include("Curso").Include(x => x.Curso.Ciclo).Include("Persona").Include(x => x.Persona.TipoPersona).Where(x => x.curso_cod.Contains(cod_cur) && x.Persona.TipoPersona.nombre.Equals("Alumno") && x.persona_id.ToString().Equals(personaid)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return docente;
        }

        public Persona obtenerreceptor(int id)
        {
            var persona = new Persona();

            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona.Where(x => x.persona_id == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return persona;
        }

        public Documento obtenerdocumento(int id)
        {
            var documento = new Documento();

            try
            {
                using (var db = new ModeloDatos())
                {
                    documento = db.Documento.Include("TipoDocumento").Include("Persona").Include("Unidad").Include("Curso").Where(x => x.documento_id == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return documento;
        }

        public void GuardarEstado()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Database.ExecuteSqlCommand(
                        "update Documento set estado = @estado where documento_id = @id",
                          new SqlParameter("estado", this.estado),
                            new SqlParameter("id", this.documento_id)
                        );
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<CursoDocente> listarcurso(int persona_id)
        {
            var curso = new List<CursoDocente>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    curso = db.CursoDocente.Include("Curso").Include(x => x.Curso.PlanEstudio.Semestre).Include(x => x.Curso.Ciclo).Include("Persona").Where(x => x.cursodocente_id > 0).Where(x => x.Persona.persona_id == persona_id).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return curso;
        }

        public List<TipoDocumento> listartipodocumento(string tipopersona)
        {
            var tipodoc = new List<TipoDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    tipodoc = db.TipoDocumento.Include("TipoPersona").Where(x => x.TipoPersona.nombre.Equals(tipopersona)).Where(x => !x.nombre.ToLower().Equals("informe final del curso") && !x.nombre.ToLower().Equals("informe prueba de entrada")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return tipodoc;
        }

        public List<TipoDocumento> listartipodocumento3(string tipopersona)
        {
            var tipodoc = new List<TipoDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    tipodoc = db.TipoDocumento.Include("TipoPersona").Where(x => x.TipoPersona.nombre.Equals(tipopersona)).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return tipodoc;
        }

        public List<Unidad> listarunidadd()
        {
            var unidad = new List<Unidad>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    unidad = db.Unidad.Include("Semestre").Where(x => x.id_unidad > 0).Where(x => x.Semestre.estado.Equals("Activo")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return unidad;
        }

        public ResponseModel GuardarArchivo(HttpPostedFileBase archivo)
        {
            var rm = new ResponseModel();
            string nombrearchivo = "";
            bool flag = false;
            bool flag2 = true;
            int codigo = 0;
            string archivogeneral = "";
            try
            {
                if (HttpContext.Current.Session["modificar"] == null || HttpContext.Current.Session["modificar"].ToString().Equals("no"))
                {
                    using (var db = new ModeloDatos())
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        var Doc = db.Entry(this);
                        Doc.State = System.Data.Entity.EntityState.Modified;
                        if (archivo != null)
                        {
                            string extension = Path.GetExtension(archivo.FileName).ToLower();
                            var filtroextension = new[] { ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt" };
                            var extensiones = Path.GetExtension(archivo.FileName);

                            TipoDocumento tipodoc = new TipoDocumento();

                            tipodoc = db.TipoDocumento.Where(x => x.tipodocumento_id == this.tipodocumento_id).SingleOrDefault();

                            DateTime dt;
                            dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                            string fecha;
                            fecha = dt.ToString("yyyy-MM-dd");

                            DateTime dtt;
                            dtt = Convert.ToDateTime(DateTime.Now.ToShortTimeString());

                            string hora;
                            hora = dtt.ToString("HH:mm:ss");

                            this.fecha_entrega = dt;
                            this.hora_entrega = dtt;

                            ConfigEntrega config = new ConfigEntrega();

                            config = db.ConfigEntrega.Where(x => x.id_unidad == this.id_unidad).SingleOrDefault();

                            DateTime dt1;
                            dt1 = Convert.ToDateTime(config.fecha_inicio);
                            dt1.ToString("yyyy/MM/dd");

                            DateTime dtt1;
                            dtt1 = Convert.ToDateTime(config.fecha_fin);

                            dtt1.ToString("yyyy/MM/dd");

                            this.fecha_entrega = dt;
                            this.hora_entrega = dtt;

                            if ((this.fecha_entrega >= dt1) && (this.fecha_entrega <= dtt1))
                            {
                                if (tipodoc.extension.Contains(extension))
                                {
                                    if (filtroextension.Contains(extensiones))
                                    {
                                        string arc = Path.GetFileName(archivo.FileName);
                                        nombrearchivo = Path.GetFileName(archivo.FileName);
                                        this.descripcion = arc;

                                        Unidad semestre = new Unidad();

                                        semestre = db.Unidad.Include("Semestre").Where(x => x.id_unidad == this.id_unidad).SingleOrDefault();

                                        this.archivo = arc + "_" + semestre.Semestre.semestre_id + "_" + this.tipodocumento_id + "_" + this.id_unidad + "_" + this.curso_cod + "_" + this.persona_id;

                                        this.tamanio = ((archivo.ContentLength) / (Math.Pow(10, 3))).ToString() + " KB";

                                        string value = extension;
                                        char delimiter = '.';
                                        string[] substrings = value.Split(delimiter);
                                        foreach (var substring in substrings)
                                        {
                                            this.extension = substring;
                                        }

                                        this.estado = "Enviado";
                                        //db.Entry(this).State = EntityState.Added;
                                        //db.SaveChanges();

                                        db.Set<Documento>().Add(this);
                                        db.SaveChanges();

                                        codigo = this.documento_id;

                                        string nuevoarchivo = "";
                                        nuevoarchivo =  this.archivo + "_" + codigo.ToString() + extension;

                                        archivogeneral = nuevoarchivo;

                                        db.Database.ExecuteSqlCommand(
                                    "update Documento set archivo = @archivo where documento_id = @documento_id",
                                    new SqlParameter("archivo", nuevoarchivo),
                                    new SqlParameter("documento_id", codigo)
                                    );

                                        HttpContext.Current.Session["id_documento"] = codigo;

                                        flag = true;
                                        flag2 = false;

                                    }
                                    else
                                    {
                                        rm.SetResponse(false, "¡Formato del archivo no válido!");
                                        return rm;
                                    }
                                }
                                else
                                {
                                    rm.SetResponse(false, "¡El formato del archivo no se admite para el tipo de documento! Subir: " + tipodoc.extension);
                                    return rm;
                                }
                            }
                            else
                            {
                                rm.SetResponse(false, "¡Ya no está disponible la entrega. Fecha fuera del rango válido!");
                                return rm;
                            }
                        }
                        else
                        {
                            rm.SetResponse(false, "¡Debe enviar un archivo!");
                            return rm;
                        }
                    }
                }
                else
                {
                    using (var db = new ModeloDatos())
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        var Doc = db.Entry(this);
                        Doc.State = System.Data.Entity.EntityState.Modified;
                        if (archivo != null)
                        {
                            string extension = Path.GetExtension(archivo.FileName).ToLower();
                            var filtroextension = new[] { ".docx", ".doc", ".xlsx", ".xls", ".pptx", ".ppt" };
                            var extensiones = Path.GetExtension(archivo.FileName);

                            TipoDocumento tipodoc = new TipoDocumento();

                            tipodoc = db.TipoDocumento.Where(x => x.tipodocumento_id == this.tipodocumento_id).SingleOrDefault();

                            DateTime dt;
                            dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                            string fecha;
                            fecha = dt.ToString("yyyy-MM-dd");

                            DateTime dtt;
                            dtt = Convert.ToDateTime(DateTime.Now.ToShortTimeString());

                            string hora;
                            hora = dtt.ToString("HH:mm:ss");

                            this.fecha_entrega = dt;
                            this.hora_entrega = dtt;

                            ConfigEntrega config = new ConfigEntrega();

                            config = db.ConfigEntrega.Where(x => x.id_unidad == this.id_unidad).SingleOrDefault();

                            DateTime dt1;
                            dt1 = Convert.ToDateTime(config.fecha_inicio);
                            dt1.ToString("yyyy/MM/dd");

                            DateTime dtt1;
                            dtt1 = Convert.ToDateTime(config.fecha_fin);

                            dtt1.ToString("yyyy/MM/dd");

                            this.fecha_entrega = dt;
                            this.hora_entrega = dtt;

                            if ((this.fecha_entrega >= dt1) && (this.fecha_entrega <= dtt1))
                            {
                                if (tipodoc.extension.Contains(extension))
                                {
                                    if (filtroextension.Contains(extensiones))
                                    {
                                        string arc = Path.GetFileName(archivo.FileName);
                                        nombrearchivo = Path.GetFileName(archivo.FileName);
                                        this.descripcion = arc;

                                        Unidad semestre = new Unidad();

                                        semestre = db.Unidad.Include("Semestre").Where(x => x.id_unidad == this.id_unidad).SingleOrDefault();

                                        this.archivo = arc + "_" + semestre.Semestre.semestre_id + "_" + this.tipodocumento_id + "_" + this.id_unidad + "_" + this.curso_cod + "_" + this.persona_id + "_" + this.documento_id + extension;

                                        archivogeneral = this.archivo;

                                        this.tamanio = ((archivo.ContentLength) / (Math.Pow(10, 3))).ToString() + " KB";

                                        string value = extension;
                                        char delimiter = '.';
                                        string[] substrings = value.Split(delimiter);
                                        foreach (var substring in substrings)
                                        {
                                            this.extension = substring;
                                        }

                                        this.estado = "Enviado";
                                        db.Entry(this).State = EntityState.Modified;
                                        db.SaveChanges();

                                        obtener(this);

                                        flag = false;
                                        flag2 = true;

                                    }
                                    else
                                    {
                                        rm.SetResponse(false, "¡Formato del archivo no válido!");
                                        return rm;
                                    }
                                }
                                else
                                {
                                    rm.SetResponse(false, "¡El formato del archivo no se admite para el tipo de documento! Subir: " + tipodoc.extension);
                                    return rm;
                                }
                            }
                            else
                            {
                                rm.SetResponse(false, "¡Ya no está disponible la entrega. Fecha fuera del rango válido!");
                                return rm;
                            }
                        }
                        else
                        {
                            rm.SetResponse(false, "¡Debe enviar un archivo!");
                            return rm;
                        }
                    }
                }

                if(flag && !flag2)
                {
                    archivo.SaveAs(HttpContext.Current.Server.MapPath("~/Server/Docs/"+TipoDocumento.nombre+"/" + archivogeneral));
                    //metadata
                    Meta meta = new Meta();
                    meta.registrarmetada(HttpContext.Current.Server.MapPath("~/Server/Docs/" + archivogeneral), this.extension, this.curso_cod, this.persona_id, this.Unidad.id_semestre, this.tipodocumento_id, this.id_unidad, this.fecha_entrega, this.tamanio);

                    rm.SetResponse(true);
                }
                else 
                {
                    HttpContext.Current.Session["antiguo"] = ultimometadata(this);
                    archivo.SaveAs(HttpContext.Current.Server.MapPath("~/Server/Docs/"  + archivogeneral));
                    //metadata
                    Meta meta = new Meta();
                    meta.registrarmetada(HttpContext.Current.Server.MapPath("~/Server/Docs/" + archivogeneral), this.extension, this.curso_cod, this.persona_id, this.Unidad.id_semestre, this.tipodocumento_id, this.id_unidad, this.fecha_entrega, this.tamanio);

                    HttpContext.Current.Session["nuevo"] = metadatanuevo(this);
                    rm.SetResponse(true);
                }

            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw e;
            }
            return rm;
        }

        public void obtener(Documento doc)
        {
            Documento obtener = new Documento();

            try
            {
                using (var db = new ModeloDatos())
                {

                    obtener = db.Documento.Where(x => x.tipodocumento_id == doc.tipodocumento_id && x.persona_id == doc.persona_id && x.id_unidad == doc.id_unidad && x.curso_cod == (doc.curso_cod) && x.archivo == (doc.archivo) && x.extension == (doc.extension) && x.tamanio == (doc.tamanio)).SingleOrDefault();
                }
            }
            catch(Exception e)
            {
                throw;
            }
            HttpContext.Current.Session["id_documento"] = obtener.documento_id;
        }

        public ResponseModel verextension(HttpPostedFileBase archivo, int tipo)
        {
            var rm = new ResponseModel();
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    var Doc = db.Entry(this);
                    Doc.State = System.Data.Entity.EntityState.Modified;
                    if (archivo != null)
                    {
                        string extension = Path.GetExtension(archivo.FileName).ToLower();

                        TipoDocumento tipodoc = new TipoDocumento();

                        tipodoc = db.TipoDocumento.Where(x => x.tipodocumento_id == tipo).SingleOrDefault();

                        if (!tipodoc.extension.Contains(extension))
                        {
                            rm.SetResponse(false, "¡El formato del archivo no se admite para el tipo de documento! Subir: " + tipodoc.extension);
                        }
                        else
                        {
                            rm.SetResponse(true);
                        }
                    }
                    else
                    {
                        rm.SetResponse(false, "¡Debe enviar un archivo!");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return rm;
        }

        public void EliminarDocumento()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Database.ExecuteSqlCommand(
                                    "delete from MetadataDocumento where documento_id = @documento_id",
                                    new SqlParameter("documento_id", this.documento_id)
                                    );

                    db.Entry(this).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void EliminarDocumentoRegistro(Documento model)
        {
            try
            {
                using (var db = new ModeloDatos())
                {

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public MetadataDocumento ultimometadata(Documento doc)
        {
            var ultimo = new MetadataDocumento();

            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    Unidad semestre = new Unidad();

                    semestre = db.Unidad.Include("Semestre").Where(x => x.id_unidad == doc.id_unidad).SingleOrDefault();

                    if(doc.extension.Equals("docx") || doc.extension.Equals("doc"))
                    {
                        ultimo = db.MetadataDocumento.Where(x => x.documento_id == doc.documento_id).OrderByDescending(x => x.metadata_id).First();
                    }
                    else if(doc.extension.Equals("xlsx") || doc.extension.Equals("xls"))
                    {
                        ultimo = db.MetadataDocumento.Where(x => x.documento_id == doc.documento_id).OrderByDescending(x => x.metadata_id).First();
                    }
                    else if (doc.extension.Equals("pptx") || doc.extension.Equals("ppt"))
                    {
                        ultimo = db.MetadataDocumento.Where(x => x.documento_id == doc.documento_id).OrderByDescending(x => x.metadata_id).First();
                    }
                }
            }
            catch(Exception e)
            {

            }

            return ultimo;
        }

        public MetadataDocumento metadatanuevo(Documento doc)
        {
            var nuevo = new MetadataDocumento();

            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    Unidad semestre = new Unidad();

                    semestre = db.Unidad.Include("Semestre").Where(x => x.id_unidad == doc.id_unidad).SingleOrDefault();

                    nuevo = db.MetadataDocumento.Where(x => x.documento_id == doc.documento_id).OrderByDescending(x => x.metadata_id).First();
                }
            }
            catch (Exception e)
            {

            }

            return nuevo;
        }

        public MetadataDocumento listarmetadatareciente(int id)
        {
            var meta = new MetadataDocumento();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    meta = db.MetadataDocumento.Include("Persona").Include("TipoDocumento").Include(x => x.Persona.TipoPersona).Where(x => x.documento_id == id).OrderByDescending(x => x.metadata_id).First();
                }
            }
            catch (Exception e)
            {

            }
            return meta;
        }

        public List<MetadataDocumento> listarmetadataantigua(int id)
        {
            int codigo = Convert.ToInt32(id);

            List<MetadataDocumento> lista = new List<MetadataDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    var data = from MetadataDocumento in db.MetadataDocumento
                               where
                                 MetadataDocumento.documento_id == id &&
                                 MetadataDocumento.metadata_id !=
                                   (from MetadataDocumento0 in db.MetadataDocumento
                                    where
       MetadataDocumento.documento_id == id
                                    select new
                                    {
                                        MetadataDocumento0.metadata_id
                                    }).Max(p => p.metadata_id)
                               orderby
                                 MetadataDocumento.metadata_id descending
                               select new
                               {
                                   metadata_id = MetadataDocumento.metadata_id,
                                   documento_id = MetadataDocumento.documento_id,
                                   cod_curso = MetadataDocumento.cod_curso,
                                   persona_id = MetadataDocumento.persona_id,
                                   semestre_id = MetadataDocumento.semestre_id,
                                   tipodocumento_id = MetadataDocumento.tipodocumento_id,
                                   id_unidad = MetadataDocumento.id_unidad,
                                   pagina_total = MetadataDocumento.pagina_total,
                                   palabra_total = MetadataDocumento.palabra_total,
                                   caracter_total = MetadataDocumento.caracter_total,
                                   linea_total = MetadataDocumento.linea_total,
                                   parrafo_total = MetadataDocumento.parrafo_total,
                                   celda = MetadataDocumento.celda,
                                   diapositiva = MetadataDocumento.diapositiva,
                                   columna = MetadataDocumento.columna,
                                   tamanio = MetadataDocumento.tamanio,
                                   programa_nombre = MetadataDocumento.programa_nombre,
                                   fecha_creacion = MetadataDocumento.fecha_creacion,
                                   fecha_subida = MetadataDocumento.fecha_subida
                               };

                    foreach (var c in data)
                    {
                        MetadataDocumento doc = new MetadataDocumento();

                        doc.metadata_id = c.metadata_id;
                        doc.documento_id = c.documento_id;
                        doc.cod_curso = c.cod_curso;
                        doc.persona_id = c.persona_id;
                        doc.semestre_id = c.semestre_id;
                        doc.tipodocumento_id = c.tipodocumento_id;
                        doc.id_unidad = c.id_unidad;
                        doc.pagina_total = c.pagina_total;
                        doc.palabra_total = c.palabra_total;
                        doc.caracter_total = c.caracter_total;
                        doc.linea_total = c.linea_total;
                        doc.parrafo_total = c.parrafo_total;
                        doc.celda = c.celda;
                        doc.diapositiva = c.diapositiva;
                        doc.columna = c.columna;
                        doc.tamanio = c.tamanio;
                        doc.programa_nombre = c.programa_nombre;
                        doc.fecha_creacion = c.fecha_creacion;
                        doc.fecha_subida = c.fecha_subida;

                        lista.Add(doc);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return lista;
        }

        public Unidad obtenersemestre(int id)
        {
            var semestre = new Unidad();

            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    semestre = db.Unidad.Include("Semestre").Where(x => x.id_unidad == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {

            }

            return semestre;
        }

        //Grafico 04
        public List<MetadataDocumento> listarmetadata(string codigo)
        {
            var meta = new List<MetadataDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    meta = db.MetadataDocumento.Include("Persona").Include("TipoDocumento").Include(x => x.Persona.TipoPersona).Where(x => x.Persona.codigo.Equals(codigo.Trim()) || x.Persona.dni.Equals(codigo.Trim())).Where(x => x.Persona.TipoPersona.nombre.Equals("Docente")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return meta;
        }

        public List<Semestre> listarsemestre()
        {
            var semestre = new List<Semestre>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    semestre = db.Semestre.ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return semestre;
        }

        public List<TipoDocumento> listartipodocumentoo(string codigopersona)
        {
            var tipodoc = new List<TipoDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    Persona persona = new Persona();
                    persona = db.Persona.Where(x => x.dni.Equals(codigopersona.Trim()) || x.codigo.Equals(codigopersona.Trim())).SingleOrDefault();
                    tipodoc = db.TipoDocumento.Include("TipoPersona").Where(x => x.tipopersona_id == persona.tipopersona_id).Where(x => x.TipoPersona.nombre.Equals("Docente")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return tipodoc;
        }

        //fin grafico 04

        //Grafico 05
        public List<Unidad> listarunidadactual()
        {
            var unidad = new List<Unidad>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    unidad = db.Unidad.Include("Semestre").Where(x => x.Semestre.estado.Equals("Activo")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return unidad;
        }

        public List<Documento> listardocumentooo(string codigopersona)
        {
            var documento = new List<Documento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    documento = db.Documento.Include("Persona").Include("Unidad").Include(x => x.Persona.TipoPersona).Include(x => x.Unidad.Semestre).Where(x => x.Unidad.Semestre.estado.Equals("Activo") && (x.Persona.codigo.Equals(codigopersona.Trim()) || x.Persona.dni.Equals(codigopersona.Trim()))).Where(x => x.Persona.TipoPersona.nombre.Equals("Docente")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return documento;
        }

        //fin grafico 05

        public string material(int ti)
        {
            var tipoo = new TipoDocumento();
            string tipo = "";
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    tipoo = db.TipoDocumento.Where(x => x.tipodocumento_id == ti).SingleOrDefault();
                    tipo = tipoo.nombre;
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return tipo;
        }

        public bool docalumno(int ti)
        {
            var tipoo = new TipoDocumento();
            bool tipo = false;
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    tipoo = db.TipoDocumento.Include("TipoPersona").Where(x => x.tipodocumento_id == ti && x.TipoPersona.nombre.Equals("Alumno")).SingleOrDefault();
                    if(tipoo.tipodocumento_id != 0)
                    {
                        tipo = true;
                    }
                    else
                    {
                        tipo = false;
                    }
                }
            }
            catch (Exception e)
            {
                //throw;
            }
            return tipo;
        }
    }
}
