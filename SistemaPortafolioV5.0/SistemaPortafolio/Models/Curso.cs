namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Data;
    using System.Data.Entity;

    [Table("Curso")]
    public partial class Curso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Curso()
        {
            CursoAlumno = new HashSet<CursoAlumno>();
            CursoDocente = new HashSet<CursoDocente>();
            Documento = new HashSet<Documento>();
            MetadataDocumento = new HashSet<MetadataDocumento>();
        }

        [Key]
        [StringLength(100)]
        public string curso_cod { get; set; }

        public int plan_id { get; set; }

        public int ciclo_id { get; set; }

        [Required]
        [StringLength(150)]
        public string nombre { get; set; }

        public int credito { get; set; }

        public int horasteoria { get; set; }

        public int horaspractica { get; set; }

        public int totalhoras { get; set; }

        [StringLength(150)]
        public string prerequisito { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        public virtual Ciclo Ciclo { get; set; }

        public virtual PlanEstudio PlanEstudio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CursoAlumno> CursoAlumno { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CursoDocente> CursoDocente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetadataDocumento> MetadataDocumento { get; set; }

        public AnexGRIDResponde ListarGrilla(int id, AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    var query = db.Curso.Include("Ciclo").Include("PlanEstudio").Where(x => x.curso_cod != "" || x.curso_cod != null);

                    if (id > 0)
                    {
                        query.Where(x => x.plan_id == id);
                    }

                    if (grilla.columna == "curso_cod")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.curso_cod)
                                                               : query.OrderBy(x => x.curso_cod);
                    }

                    if (grilla.columna == "ciclo_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.ciclo_id)
                                                               : query.OrderBy(x => x.ciclo_id);
                    }

                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                               : query.OrderBy(x => x.nombre);
                    }

                    if (grilla.columna == "prerequisito")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.prerequisito)
                                                               : query.OrderBy(x => x.prerequisito);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "nombre" && f.valor != "")
                            query = query.Where(x => x.ciclo_id.ToString() == f.valor);

                        if (f.columna == "nombrecurso")
                            query = query.Where(x => x.nombre.StartsWith(f.valor));

                        if (f.columna == "plan_estudio")
                        {
                            var plan_id = Int32.Parse(f.valor);

                            query = query.Where(x => x.plan_id == plan_id);
                        }
                            
                    }


                    var curso = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in curso
                                   select new
                                   {
                                       s.curso_cod,
                                       plan_estudio = s.PlanEstudio.nombre,
                                       s.Ciclo.nombre,
                                       nombrecurso = s.nombre,
                                       s.prerequisito
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

        public void GuardarNuevo()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    var LastRegister = 0;

                    var flag = false;

                    var registro = new CursoDocente();

                    if (this.curso_cod != null)
                    {
                        db.Database.ExecuteSqlCommand(
                        "DELETE FROM CursoDocente WHERE curso_cod = @curso_cod",
                        new SqlParameter("curso_cod", this.curso_cod)
                        );

                        try
                        {
                            LastRegister = db.CursoDocente
                                    .OrderByDescending(x => x.cursodocente_id)
                                    .First().cursodocente_id;
                        }
                        catch (Exception e)
                        {
                            LastRegister = 0;
                        }

                        registro = db.CursoDocente
                                    .Where(x => x.curso_cod.Contains(this.curso_cod)).SingleOrDefault();

                        var d = db.Curso
                                    .Where(x => x.curso_cod.Contains(this.curso_cod)).SingleOrDefault();

                        var cursodocente = this.CursoDocente;

                        if (d == null)
                        {
                            flag = true;
                            this.CursoDocente = null;
                            db.Database.ExecuteSqlCommand(
                            "insert into Curso values(@curso_cod,@plan_id,@ciclo_id,@nombre,@credito,@horasteoria,@horaspractica,@totalhoras,@prerequisito,@estado)",
                            new SqlParameter("curso_cod", this.curso_cod),
                            new SqlParameter("plan_id", this.plan_id),
                            new SqlParameter("ciclo_id", this.ciclo_id),
                            new SqlParameter("nombre", this.nombre),
                            new SqlParameter("credito", this.credito),
                            new SqlParameter("horasteoria", this.horasteoria),
                            new SqlParameter("horaspractica", this.horaspractica),
                            new SqlParameter("totalhoras", this.totalhoras),
                            new SqlParameter("prerequisito", this.prerequisito),
                            new SqlParameter("estado", this.estado)
                            );
                            this.CursoDocente = cursodocente;

                            foreach (var c in this.CursoDocente)
                            {
                                db.Database.ExecuteSqlCommand(
                            "insert into CursoDocente values(@curso_cod,@persona_id)",
                            new SqlParameter("curso_cod", this.curso_cod),
                            new SqlParameter("persona_id", c.persona_id)
                            );
                            }
                        }
                        else
                        {
                            this.CursoDocente = null;
                            db.Entry(this).State = EntityState.Modified;
                            this.CursoDocente = cursodocente;
                        }
                    }
                    else
                    {
                        registro = db.CursoDocente
                                    .Where(x => x.curso_cod.Contains(this.curso_cod)).SingleOrDefault();
                        db.Entry(this).State = EntityState.Added;
                    }

                    if (flag)
                    {

                    }
                    else
                    {
                        foreach (var c in this.CursoDocente)
                        {
                            if (registro == null)
                            {
                                c.cursodocente_id = LastRegister + 1;
                                db.Entry(c).State = EntityState.Added;
                            }
                            else
                            {
                                c.cursodocente_id = registro.cursodocente_id;
                                db.Entry(c).State = EntityState.Unchanged;
                            }
                        }

                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void GuardarModificar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    var LastRegister = 0;

                    var flag = false;

                    var registro = new CursoDocente();

                    if (this.curso_cod != null)
                    {
                        db.Database.ExecuteSqlCommand(
                        "DELETE FROM CursoDocente WHERE curso_cod = @curso_cod",
                        new SqlParameter("curso_cod", this.curso_cod)
                        );

                        try
                        {
                            LastRegister = db.CursoDocente
                                    .OrderByDescending(x => x.cursodocente_id)
                                    .First().cursodocente_id;
                        }
                        catch (Exception e)
                        {
                            LastRegister = 0;
                        }

                        registro = db.CursoDocente
                                    .Where(x => x.curso_cod.Contains(this.curso_cod)).SingleOrDefault();

                        var cursodocente = this.CursoDocente;
                        this.CursoDocente = null;
                        db.Entry(this).State = EntityState.Modified;
                        this.CursoDocente = cursodocente;
                    }
                    else
                    {
                        registro = db.CursoDocente
                                    .Where(x => x.curso_cod.Contains(this.curso_cod)).SingleOrDefault();
                        db.Entry(this).State = EntityState.Added;
                    }

                    foreach (var c in this.CursoDocente)
                    {
                        if (registro == null)
                        {
                            c.cursodocente_id = LastRegister + 1;
                            db.Entry(c).State = EntityState.Added;
                        }
                        else
                        {
                            c.cursodocente_id = registro.cursodocente_id;
                            db.Entry(c).State = EntityState.Unchanged;
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Eliminar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Database.ExecuteSqlCommand(
                            "delete from CursoDocente where curso_cod = @curso_cod",
                            new SqlParameter("curso_cod", this.curso_cod)
                            );
                    db.Entry(this).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Curso Obtener(string id)
        {
            var curso = new Curso();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.Curso.Include("Ciclo").Include("PlanEstudio").
                        Where(x => x.curso_cod.Contains(id)).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso;
        }

        public CursoDocente ObtenerCursoDocente(string id)
        {
            var curso = new CursoDocente();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.CursoDocente.Include("Curso").Include("Persona").
                        Where(x => x.curso_cod.Contains(id)).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso;
        }

        public string obtenerplan(int id)
        {
            var curso = new PlanEstudio();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.PlanEstudio.
                        Where(x => x.plan_id == id).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso.nombre;
        }

        public List<Curso> listarcurso(int id)
        {
            var curso = new List<Curso>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.Curso.Include("PlanEstudio").
                        Where(x => x.plan_id == id).
                        ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso;
        }

        public List<CursoDocente> cursoal(int id)
        {
            var curso = new List<CursoDocente>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    curso = db.CursoDocente.Include("Curso").Include(x => x.Curso.PlanEstudio).Include(x => x.Curso.Ciclo).
                        Where(x => x.persona_id == id).
                        ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return curso;
        }

        public CursoDocente listardocente(string id)
        {
            var cursodocente = new CursoDocente();

            try
            {
                using (var db = new ModeloDatos())
                {
                    cursodocente = db.CursoDocente.Include("Persona").Where(x => x.curso_cod.Contains(id)).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return cursodocente;
        }

        public List<Persona> listartododocente()
        {
            var docente = new List<Persona>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    docente = db.Persona.Include("TipoPersona").Where(x => x.TipoPersona.nombre.Contains("Docente") || x.TipoPersona.nombre.Contains("Administrador")).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return docente;
        }

        //conculta01
        public AnexGRIDResponde ListarGrillac01(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.Curso.Include("TipoPersona").Where(x => x.plan_id > 0);

                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "persona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.plan_id)
                                                    : query.OrderBy(x => x.plan_id);
                    }
                    if (grilla.columna == "tipopersona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.ciclo_id)
                                                    : query.OrderBy(x => x.ciclo_id);
                    }

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.curso_cod)
                                                    : query.OrderBy(x => x.curso_cod);
                    }
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                    : query.OrderBy(x => x.nombre);
                    }
                    if (grilla.columna == "apellido")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.credito)
                                                    : query.OrderBy(x => x.credito);
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
                            query = query.Where(x => x.plan_id.ToString() == f.valor);
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.curso_cod,
                                m.PlanEstudio.nombre,

                                m.ciclo_id,
                                nombrep = m.nombre + " " + m.credito,

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

        public List<PlanEstudio> listartipo() //retornar es un Collection
        {
            var tipo = new List<PlanEstudio>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    tipo = db.PlanEstudio.ToList();
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
