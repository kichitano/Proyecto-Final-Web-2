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

    [Table("Unidad")]
    public partial class Unidad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unidad()
        {
            ConfigEntrega = new HashSet<ConfigEntrega>();
            Documento = new HashSet<Documento>();
            MetadataDocumento = new HashSet<MetadataDocumento>();
        }

        [Key]
        public int id_unidad { get; set; }

        public int id_semestre { get; set; }

        [StringLength(150)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigEntrega> ConfigEntrega { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        public virtual Semestre Semestre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetadataDocumento> MetadataDocumento { get; set; }
        //


        public List<Semestre> listarsemestre()//retornar un objeto
        {
            var semestre = new List<Semestre>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    semestre = db.Semestre
                            .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return semestre;
        }

        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    var query = db.Unidad.Include("Semestre").Where(x => x.id_unidad > 0);

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.id_unidad)
                                                               : query.OrderBy(x => x.id_unidad);
                    }

                    if (grilla.columna == "semestre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.Semestre.semestre_id)
                                                               : query.OrderBy(x => x.Semestre.semestre_id);
                    }

                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.descripcion)
                                                               : query.OrderBy(x => x.descripcion);
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
                            query = query.Where(x => x.descripcion.Contains(f.valor));
                        if (f.columna == "semestre" && f.valor != "")
                            query = query.Where(x => x.Semestre.semestre_id.ToString().Contains(f.valor));
                    }


                    var unidad = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in unidad
                                   select new
                                   {
                                       codigo = s.id_unidad,
                                       semestre=s.Semestre.nombre,
                                       nombre = s.descripcion,
                                       s.estado,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public Unidad Obtener(int id)
        {
            var unidad = new Unidad();

            try
            {
                using (var db = new ModeloDatos())
                {
                    unidad = db.Unidad.
                        Where(x => x.id_unidad == id).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return unidad;
        }

        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.id_unidad > 0)
                    {
                        db.Entry(this).State = EntityState.Modified;
                    }
                    else
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
