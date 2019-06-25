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

    [Table("ConfigEntrega")]
    public partial class ConfigEntrega
    {
        [Key]
        public int configentrega_id { get; set; }

        public int id_unidad { get; set; }

        [StringLength(150)]
        public string nombre { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_inicio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_fin { get; set; }

        [Required]
        [StringLength(50)]
        public string estado { get; set; }

        public virtual Unidad Unidad { get; set; }
        //
        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    var query = db.ConfigEntrega.Include("Unidad").Include(x => x.Unidad.Semestre).Where(x => x.configentrega_id > 0);

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.configentrega_id)
                                                               : query.OrderBy(x => x.configentrega_id);
                    }

                    if (grilla.columna == "semestre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Unidad.Semestre.semestre_id)
                                                               : query.OrderBy(x => x.Unidad.Semestre.semestre_id);
                    }

                    if (grilla.columna == "unidad")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Unidad.descripcion)
                                                               : query.OrderBy(x => x.Unidad.descripcion);
                    }

                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                               : query.OrderBy(x => x.nombre);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "nombre" && f.valor != "")
                            query = query.Where(x => x.nombre.Contains(f.valor));

                        if (f.columna == "semestre" && f.valor != "")
                            query = query.Where(x => x.Unidad.Semestre.semestre_id.ToString().Contains(f.valor));
                    }


                    var configentrega = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in configentrega
                                   select new
                                   {
                                       codigo = s.configentrega_id,
                                       semestre = s.Unidad.Semestre.nombre,
                                       unidad = s.Unidad.descripcion,
                                       s.nombre,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public List<Unidad> listarunidad()
        {
            var unidad = new List<Unidad>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    unidad = db.Unidad.Include("Semestre").ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return unidad;
        }

        public ConfigEntrega Obtener(int id)
        {
            var configuracion = new ConfigEntrega();

            try
            {
                using (var db = new ModeloDatos())
                {
                    configuracion = db.ConfigEntrega.Include("Unidad").Include(x => x.Unidad.Semestre).
                        Where(x => x.configentrega_id == id).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return configuracion;
        }

        public void Guardar()
        {
            System.DateTime dt;
            dt = Convert.ToDateTime(this.fecha_inicio);

            string fecha_inicio;
            fecha_inicio = dt.ToString("yyyyMMdd");

            System.DateTime dtt;
            dtt = Convert.ToDateTime(this.fecha_fin);

            string fecha_fin;
            fecha_fin = dtt.ToString("yyyyMMdd");

            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.configentrega_id > 0)
                    {
                        db.Database.ExecuteSqlCommand(
                        "update ConfigEntrega set id_unidad = @id_unidad, nombre = @nombre, fecha_inicio = cast(@fecha_inicio as smalldatetime), fecha_fin = cast(@fecha_fin as smalldatetime), estado = @estado where configentrega_id = @id",
                        new SqlParameter("id_unidad", this.id_unidad),
                        new SqlParameter("nombre", this.nombre),
                        new SqlParameter("fecha_inicio", this.fecha_inicio),
                          new SqlParameter("fecha_fin", this.fecha_fin),
                          new SqlParameter("estado", this.estado),
                            new SqlParameter("id", this.configentrega_id)
                        );

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand(
                        "insert into ConfigEntrega values(@id_unidad,@nombre,cast(@fecha_inicio as smalldatetime),cast(@fecha_fin as smalldatetime),@estado)",
                        new SqlParameter("id_unidad", this.id_unidad),
                        new SqlParameter("nombre", this.nombre),
                        new SqlParameter("fecha_inicio", this.fecha_inicio),
                          new SqlParameter("fecha_fin", this.fecha_fin),
                          new SqlParameter("estado", this.estado)
                        );
                    }

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
                    db.Entry(this).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
