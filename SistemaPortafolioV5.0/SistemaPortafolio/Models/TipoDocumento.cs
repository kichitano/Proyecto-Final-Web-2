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

    [Table("TipoDocumento")]
    public partial class TipoDocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoDocumento()
        {
            Documento = new HashSet<Documento>();
            MetadataDocumento = new HashSet<MetadataDocumento>();
        }

        [Key]
        public int tipodocumento_id { get; set; }

        public int tipopersona_id { get; set; }

        [Required]
        [StringLength(150)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string extension { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documento> Documento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetadataDocumento> MetadataDocumento { get; set; }

        public virtual TipoPersona TipoPersona { get; set; }
        //
        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();

                    var query = db.TipoDocumento.Include("TipoPersona").Where(x => x.tipodocumento_id > 0);

                    if (grilla.columna == "codigo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipodocumento_id)
                                                               : query.OrderBy(x => x.tipodocumento_id);
                    }

                    if (grilla.columna == "persona")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipopersona_id)
                                                               : query.OrderBy(x => x.tipopersona_id);
                    }

                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                               : query.OrderBy(x => x.nombre);
                    }

                    if (grilla.columna == "extension")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.extension)
                                                               : query.OrderBy(x => x.extension);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "nombre" && f.valor != "")
                            query = query.Where(x => x.nombre.Contains(f.valor));
                    }


                    var tipodocumento = query.Skip(grilla.pagina)
                                      .Take(grilla.limite)
                                      .ToList();

                    var total = query.Count();

                    grilla.SetData(from s in tipodocumento
                                   select new
                                   {
                                       codigo = s.tipodocumento_id,
                                       persona = s.TipoPersona.nombre,
                                       s.nombre,
                                       s.extension,
                                   }, total);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return grilla.responde();
        }

        public TipoDocumento Obtener(int id)
        {
            var tipodocumento = new TipoDocumento();

            try
            {
                using (var db = new ModeloDatos())
                {
                    tipodocumento = db.TipoDocumento.
                        Where(x => x.tipodocumento_id == id).
                        SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return tipodocumento;
        }

        public List<TipoPersona> listartipopersona()
        {
            var tipopersona = new List<TipoPersona>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    tipopersona = db.TipoPersona.ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return tipopersona;
        }

        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if(this.tipodocumento_id > 0)
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
