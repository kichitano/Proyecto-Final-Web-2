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

    [Table("Ciclo")]
    public partial class Ciclo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ciclo()
        {
            Curso = new HashSet<Curso>();
        }

        [Key]
        public int ciclo_id { get; set; }

        [StringLength(100)]
        public string nombre { get; set; }

        [Column(TypeName = "text")]
        public string descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Curso> Curso { get; set; }

        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.Ciclo.Where(x => x.ciclo_id > 0);
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
                    if (grilla.columna == "descripcion")
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
                            query = query.Where(x => x.nombre.Contains(f.valor));
                    }
                    var ciclo = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();
                    var total = query.Count();
                    grilla.SetData(from s in ciclo
                                   select new
                                   {
                                       s.ciclo_id,
                                       s.nombre,
                                       s.descripcion,
                                       s.estado
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return grilla.responde();
        }

        public Ciclo Obtener(int id)
        {
            var ciclo = new Ciclo();
            try
            {
                using (var db = new ModeloDatos())
                {
                    ciclo = db.Ciclo.
                        Where(x => x.ciclo_id == id).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return ciclo;
        }
        //metodo guardar
        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.ciclo_id > 0)
                    {
                        db.Entry(this).State = EntityState.Modified;
                    }
                    //si no existiera
                    else
                    {
                        db.Entry(this).State = EntityState.Added;
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
    }
}
