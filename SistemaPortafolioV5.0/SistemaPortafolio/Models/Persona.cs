namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.SqlClient;
    using System.IO;
    using System.Web;

    [Table("Persona")]
    public partial class Persona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persona()
        {
            CursoAlumno = new HashSet<CursoAlumno>();
            CursoDocente = new HashSet<CursoDocente>();
            Documento = new HashSet<Documento>();
            HojaVida = new HashSet<HojaVida>();
            MetadataDocumento = new HashSet<MetadataDocumento>();
            Notificacion = new HashSet<Notificacion>();
            Notificacion1 = new HashSet<Notificacion>();
            Usuario = new HashSet<Usuario>();
        }

        [Key]
        public int persona_id { get; set; }

        public int tipopersona_id { get; set; }

        [Required]
        [StringLength(8)]
        public string dni { get; set; }

        [StringLength(10)]
        public string codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(15)]
        public string celular { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CursoAlumno> CursoAlumno { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CursoDocente> CursoDocente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVida> HojaVida { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetadataDocumento> MetadataDocumento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notificacion> Notificacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notificacion> Notificacion1 { get; set; }

        public virtual TipoPersona TipoPersona { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuario { get; set; }

        public List<Persona> Listar()
        {
            var persona = new List<Persona>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return persona;
        }

        public Persona ObtenerPersona(string codigo)
        {
            var persona = new Persona();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona.Where(x => x.codigo.Equals(codigo.Trim()) || x.dni.Equals(codigo.Trim())).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return persona;
        }

        //para la anexgrid
        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.Persona.Include("TipoPersona").Include("Usuario").Where(x => x.persona_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "persona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.persona_id)
                                                    : query.OrderBy(x => x.persona_id);
                    }
                    if (grilla.columna == "tipopersona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipopersona_id)
                                                    : query.OrderBy(x => x.tipopersona_id);
                    }
                    if (grilla.columna == "dni")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.dni)
                                                    : query.OrderBy(x => x.dni);
                    }
                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.codigo)
                                                    : query.OrderBy(x => x.codigo);
                    }
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                    : query.OrderBy(x => x.nombre);
                    }
                    if (grilla.columna == "apellido")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.apellido)
                                                    : query.OrderBy(x => x.apellido);
                    }
                    if (grilla.columna == "email")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.email)
                                                    : query.OrderBy(x => x.email);
                    }
                    if (grilla.columna == "celular")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.celular)
                                                    : query.OrderBy(x => x.celular);
                    }
                    if (grilla.columna == "estado")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.estado)
                                                    : query.OrderBy(x => x.estado);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "apel_per")
                            query = query.Where(x => x.apellido.Contains(f.valor));
                        if (f.columna == "nomb_per")
                            query = query.Where(x => x.nombre.Contains(f.valor));
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.persona_id,
                                personatipo = m.TipoPersona.nombre,
                                m.dni,
                                m.codigo,
                                m.nombre,
                                m.apellido,
                                m.email,
                                m.celular,
                                m.estado,
                            },
                            total
                        );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return grilla.responde();
        }

        //Consulta Cantidad de Hojas de Vida Docente
        public AnexGRIDResponde ListarGrillaCCHVD(AnexGRID grilla, string codigo)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    Persona person = new Persona();
                    person = db.Persona.Where(x => x.codigo.Equals(codigo) || x.dni.Equals(codigo)).SingleOrDefault();

                    HojaVida hojav = new Models.HojaVida();
                    hojav = db.HojaVida.Include("Persona").Where(x => x.persona_id == person.persona_id).SingleOrDefault();

                    var query = (from HojaVidaDocenteCRP in
(from HojaVidaDocenteCRP in db.HojaVidaDocenteCRP
 where
    HojaVidaDocenteCRP.hojavida_id ==
      ((from HojaVidaDocenteCRP0 in db.HojaVidaDocenteCRP
        where
             HojaVidaDocenteCRP0.hojavida_id == hojav.hojavida_id
        select new
        {
            HojaVidaDocenteCRP0.hojavida_id
        }).FirstOrDefault().hojavida_id)
 select new
 {
     HojaVidaDocenteCRP.hojavida_id,
     Dummy = "x"
 })
                                 group HojaVidaDocenteCRP by new { HojaVidaDocenteCRP.Dummy } into g
                                 select new
                                 {
                                     Column1 = "Certificaciones",
                                     total = g.Count(p => p.Dummy != null)
                                 }
).Union
(from HojaVidaDocenteEX in
(from HojaVidaDocenteEX in db.HojaVidaDocenteEX
 where
    HojaVidaDocenteEX.hojavida_id ==
      ((from HojaVidaDocenteEX0 in db.HojaVidaDocenteEX
        where
             HojaVidaDocenteEX0.hojavida_id == hojav.hojavida_id
        select new
        {
            HojaVidaDocenteEX0.hojavida_id
        }).FirstOrDefault().hojavida_id)
 select new
 {
     HojaVidaDocenteEX.hojavida_id,
     Dummy = "x"
 })
 group HojaVidaDocenteEX by new { HojaVidaDocenteEX.Dummy } into g
 select new
 {
     Column1 = "Experiencias",
     total = g.Count(p => p.Dummy != null)
 }
).Union
(from HojaVidaDocenteFA in
(from HojaVidaDocenteFA in db.HojaVidaDocenteFA
 where
    HojaVidaDocenteFA.hojavida_id ==
      ((from HojaVidaDocenteFA0 in db.HojaVidaDocenteFA
        where
             HojaVidaDocenteFA0.hojavida_id == hojav.hojavida_id
        select new
        {
            HojaVidaDocenteFA0.hojavida_id
        }).FirstOrDefault().hojavida_id)
 select new
 {
     HojaVidaDocenteFA.hojavida_id,
     Dummy = "x"
 })
 group HojaVidaDocenteFA by new { HojaVidaDocenteFA.Dummy } into g
 select new
 {
     Column1 = "Formacion Academica",
     total = g.Count(p => p.Dummy != null)
 }
).Union
(from HojaVidaDocenteFC in
(from HojaVidaDocenteFC in db.HojaVidaDocenteFC
 where
    HojaVidaDocenteFC.hojavida_id ==
      ((from HojaVidaDocenteFC0 in db.HojaVidaDocenteFC
        where
             HojaVidaDocenteFC0.hojavida_id == hojav.hojavida_id
        select new
        {
            HojaVidaDocenteFC0.hojavida_id
        }).FirstOrDefault().hojavida_id)
 select new
 {
     HojaVidaDocenteFC.hojavida_id,
     Dummy = "x"
 })
 group HojaVidaDocenteFC by new { HojaVidaDocenteFC.Dummy } into g
 select new
 {
     Column1 = "Formacion Continua",
     total = g.Count(p => p.Dummy != null)
 }
);

                    //var query = db.HojaVidaDocenteCRP.Concat(db.HojaVidaDocenteEX).Concat(db.HojaVidaDocenteFA).Concat(db.HojaVidaDocenteFC);

                    //query.GroupBy(info => info.docente)
                    //    .Select(group => new {
                    //        docente = group.Key,
                    //        cantidad = group.Count()
                    //    });

                    //var query = db.Persona.Include("TipoPersona").Include("Usuario").Where(x => x.TipoPersona.nombre.Equals("Docente"));
                    //obtener los campos y que permita ordenar
                    /*
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => (x.nombre + " " + x.apellido))
                                                    : query.OrderBy(x => (x.nombre + " " + x.apellido));
                    }
                    if (grilla.columna == "cantidad")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.dni)
                                                    : query.OrderBy(x => x.dni);
                    }
                    */

                    //var cargo = query.Skip(grilla.pagina)
                    //                .Take(grilla.limite)
                    //                .ToList();

                    //var total = query.Count();//cantidad de registros

                    //grilla.SetData(
                    //        query,
                    //        total
                    //    );

                    var hoja = query.ToList();

                    var total = query.Count();

                    grilla.SetData(from s in hoja
                                   select new
                                   {
                                       descripcion = s.Column1,
                                       cantidad =  s.total,
                                   }, total);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return grilla.responde();
        }
        public AnexGRIDResponde ListarGrillaA(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.Persona.Include("TipoPersona").Include("Usuario").Where(x => x.persona_id > 0 && x.TipoPersona.nombre.Equals("Alumno"));
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.persona_id)
                                                    : query.OrderBy(x => x.persona_id);
                    }
                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.codigo)
                                                    : query.OrderBy(x => x.codigo);
                    }
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => (x.nombre + " " + x.apellido))
                                                    : query.OrderBy(x => (x.nombre + " " + x.apellido));
                    }
                    if (grilla.columna == "dni")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.dni)
                                                    : query.OrderBy(x => x.dni);
                    }
                    if (grilla.columna == "estado")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.estado)
                                                    : query.OrderBy(x => x.estado);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "nombre")
                            query = query.Where(x => (x.nombre + " " + x.apellido).Contains(f.valor));
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                id = m.persona_id,
                                m.codigo,
                                nombre = m.nombre + " " + m.apellido,
                                m.dni,
                                m.estado,
                            },
                            total
                        );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return grilla.responde();
        }

        //metodo obtener
        public Persona Obtener(int id)//retornar un objeto
        {
            var persona = new Persona();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona.Include("Usuario").Include("TipoPersona")
                        .Where(x => x.persona_id == id)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return persona;
        }
        //metodo buscar
        public List<Persona> Buscar(string criterio)//retornar un objeto
        {
            var persona = new List<Persona>();
            String estado = "";
            if (criterio == "Activo") estado = "Activo";
            if (criterio == "Inactivo") estado = "Inactivo";
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.Persona
                            .Where(x => x.nombre.Contains(criterio) || x.estado == estado)
                            .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return persona;
        }
        //METODO GUARDAR
        public void Guardar(string SiDocente, string siSemestre, bool docente)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.persona_id > 0)
                    {
                        db.Entry(this).State = EntityState.Modified;
                        Directory.Move(
                            HttpContext.Current.Server.MapPath("~/Server/EPIS/Portafolio/Portafolio_" + siSemestre + "/" + SiDocente),
                            HttpContext.Current.Server.MapPath("~/Server/EPIS/Portafolio/Portafolio_" + siSemestre + "/" + this.dni + "_" + this.nombre + " " + this.apellido));

                    }
                    else
                    {
                        db.Entry(this).State = EntityState.Added;
                        if (docente)
                        {
                            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Server/EPIS/Portafolio/Portafolio_" + siSemestre), 
                                this.dni + "_" + this.nombre + " " + this.apellido);
                            Directory.CreateDirectory(path);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //metodo eliminar
        public void Eliminar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Entry(this).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //consulta una Persona cuantos usuarios tiene 
        public string[,] Consulta()
        {
            string[,] criterios;
            try
            {
                using (var db = new ModeloDatos())
                {
                    var per = (from m in db.Persona
                               join c in db.Usuario on
                               m.persona_id equals c.persona_id into r_join
                               from c in r_join.DefaultIfEmpty()
                               group new { m, c } by new
                               {
                                   m.persona_id,
                                   m.nombre
                               } into g
                               select new
                               {
                                   id_persona = (int?)g.Key.persona_id,
                                   g.Key.nombre,
                                   Cantidad = g.Count(p => p.c.persona_id != null)
                               }).ToList();
                    criterios = new string[per.Count(), 2];
                    int count = 0;
                    foreach (var m in per)
                    {
                        criterios[count, 0] = Convert.ToString(m.nombre);
                        criterios[count, 1] = Convert.ToString(m.Cantidad);
                        count++;
                    }
                }
                return criterios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseModel agregarcurso(string codigo, int persona)
        {
            var rm = new ResponseModel();
            try
            {
                using (var db = new ModeloDatos())
                {
                    CursoDocente cursos = new CursoDocente();

                    cursos = db.CursoDocente.Where(x => x.curso_cod.Equals(codigo) && x.persona_id == persona).SingleOrDefault();

                    if (cursos != null)
                    {
                        rm.SetResponse(false, "¡Ya está registrado el curso!");
                    }
                    else
                    {
                        Curso curso = new Curso();

                        curso = db.Curso.Include("PlanEstudio").Where(x => x.PlanEstudio.estado.Equals("Activo") && x.curso_cod.Equals(codigo)).SingleOrDefault();

                        if (curso.curso_cod == null || curso.curso_cod.Equals(""))
                        {
                            rm.SetResponse(false, "¡El curso no está disponible en el actual Plan de Estudio actual!");
                        }
                        else
                        {
                            db.Database.ExecuteSqlCommand(
                                "insert into CursoDocente values(@curso_cod,@persona_id)",
                                new SqlParameter("curso_cod", codigo),
                                new SqlParameter("persona_id", persona)
                                );
                            rm.SetResponse(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rm.SetResponse(false,"Ocurrió un error al insertar el curso");
            }
            return rm;
        }

        public void eliminarcurso(int id)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Database.ExecuteSqlCommand(
                                "delete from CursoDocente where cursodocente_id = @id",
                                new SqlParameter("id", id)
                                );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //conculta01
        public AnexGRIDResponde ListarGrillac01(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.Persona.Include("TipoPersona").Where(x => x.persona_id > 0);

                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "persona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.persona_id)
                                                    : query.OrderBy(x => x.persona_id);
                    }
                    if (grilla.columna == "tipopersona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipopersona_id)
                                                    : query.OrderBy(x => x.tipopersona_id);
                    }

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.codigo)
                                                    : query.OrderBy(x => x.codigo);
                    }
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                    : query.OrderBy(x => x.nombre);
                    }
                    if (grilla.columna == "apellido")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.apellido)
                                                    : query.OrderBy(x => x.apellido);
                    }

                    if (grilla.columna == "estado")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.estado)
                                                    : query.OrderBy(x => x.estado);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "nombre" && f.valor != "")
                            query = query.Where(x => x.tipopersona_id.ToString() == f.valor);
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.persona_id,
                                m.TipoPersona.nombre,

                                m.codigo,
                                nombrep = m.nombre + " " + m.apellido,

                                m.estado,
                            },
                            total
                        );
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return grilla.responde();
        }
        public List<TipoPersona> listartipo() //retornar es un Collection
        {
            var tipo = new List<TipoPersona>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    tipo = db.TipoPersona.ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return tipo;
        }
    }
}
