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

    [Table("Notificacion")]
    public partial class Notificacion
    {
        [Key]
        public int notificacion_id { get; set; }

        public int persona_emisor { get; set; }

        public int persona_receptor { get; set; }

        [Required]
        [StringLength(150)]
        public string titulo { get; set; }

        [Required]
        [StringLength(150)]
        public string asunto { get; set; }

        [Required]
        [StringLength(150)]
        public string mensaje { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_emision { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual Persona Persona1 { get; set; }

        public AnexGRIDResponde ListarGrilla(AnexGRID grilla, int id_persona)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    //incluir varias tablas de otras tablas, codigo en la siguiente linea
                    var query = db.Notificacion.Include("Persona").Include("Persona1").Where(x => x.notificacion_id > 0).Where(x => x.Persona1.persona_id == id_persona);

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.notificacion_id)
                                                               : query.OrderBy(x => x.notificacion_id);
                    }

                    if (grilla.columna == "emisor")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Persona.persona_id)
                                                               : query.OrderBy(x => x.Persona.persona_id);
                    }

                    if (grilla.columna == "titulo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.titulo)
                                                               : query.OrderBy(x => x.titulo);
                    }

                    if (grilla.columna == "asunto")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.asunto)
                                                               : query.OrderBy(x => x.asunto);
                    }

                    if (grilla.columna == "fecha")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_emision)
                                                               : query.OrderBy(x => x.fecha_emision);
                    }

                    if (grilla.columna == "estado")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.estado)
                                                               : query.OrderBy(x => x.estado);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "asunto" && f.valor != "")
                            query = query.Where(x => x.asunto.Contains(f.valor));
                    }


                    var persona = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in persona
                                   select new
                                   {
                                       codigo = s.notificacion_id,
                                       emisor = s.Persona.nombre + " " + s.Persona.apellido,
                                       titulo = s.titulo,
                                       asunto = s.asunto,
                                       fecha = s.fecha_emision,
                                       estado = s.estado,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public void Guardar()
        {
            System.DateTime dt;
            dt = Convert.ToDateTime(DateTime.Now);

            this.fecha_emision = dt;
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.notificacion_id == 0)
                    {
                        db.Entry(this).State = EntityState.Added;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Notificacion> ListarNotificacion(int id)
        {
            var notificacion = new List<Notificacion>();
            var usuario = new Usuario();

            try
            {
                using (var db = new ModeloDatos())
                {
                    usuario = db.Usuario.Include("Persona").Where(x => x.usuario_id == id).SingleOrDefault();
                    notificacion = db.Notificacion.Include("Persona").Include("Persona1").Where(x => x.persona_receptor == usuario.Persona.persona_id).OrderByDescending(x => x.fecha_emision).Take(6).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return notificacion;
        }

        public List<Notificacion> ListarNotificacionNoLeido(int id)
        {
            var notificacion = new List<Notificacion>();
            var usuario = new Usuario();

            try
            {
                using (var db = new ModeloDatos())
                {
                    usuario = db.Usuario.Include("Persona").Where(x => x.usuario_id == id).SingleOrDefault();
                    notificacion = db.Notificacion.Include("Persona").Include("Persona1").Where(x => x.persona_receptor == usuario.Persona.persona_id).Where(x => x.estado.Equals("No Leido")).OrderByDescending(x => x.fecha_emision).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return notificacion;
        }

        public Notificacion obtener(int id)
        {
            var notificacion = new Notificacion();

            try
            {
                using (var db = new ModeloDatos())
                {
                    notificacion = db.Notificacion.Include("Persona").Include("Persona1").Where(x => x.notificacion_id == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return notificacion;
        }

        public void cambiarleido(int id)
        {
            using (var db = new ModeloDatos())
            {
                db.Database.ExecuteSqlCommand(
                    "update Notificacion set estado = @estado where notificacion_id = @notificacion_id",
                    new SqlParameter("estado", "Leido"),
                    new SqlParameter("notificacion_id", id)
                    );
            }
        }

        //consulta04

        public List<Notificacion> Listar()//retornar es un Collection
        {
            var notificacion = new List<Notificacion>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    notificacion = db.Notificacion.ToList();

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return notificacion;
        }
        public List<Notificacion> Buscar(string criterio) //collection
        {
            var notificacion = new List<Notificacion>();


            try
            {
                using (var db = new ModeloDatos())
                {
                    notificacion = db.Notificacion.Include("Persona").Where(x => x.Persona.codigo.Contains(criterio.Trim()) || x.Persona.dni.Contains(criterio.Trim())).ToList();

                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return notificacion;
        }
    }
}
