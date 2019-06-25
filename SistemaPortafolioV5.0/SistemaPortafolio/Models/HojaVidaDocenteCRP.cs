namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;

    [Table("HojaVidaDocenteCRP")]
    public partial class HojaVidaDocenteCRP
    {
        [Key]
        public int hojavidadocentecrp_id { get; set; }

        public int hojavida_id { get; set; }

        [Required]
        [StringLength(150)]
        public string certificacion { get; set; }

        [Required]
        [StringLength(150)]
        public string institucion { get; set; }

        [Required]
        [StringLength(20)]
        public string año { get; set; }

        public virtual HojaVida HojaVida { get; set; }

        public List<HojaVidaDocenteCRP> Listar(int codigo)
        {
            var rango = new List<HojaVidaDocenteCRP>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    rango = db.HojaVidaDocenteCRP.Where(x => x.HojaVida.persona_id == codigo).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return rango;
        }
        //para la anexgrid
        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    grilla.Inicializar();
                    var query = db.HojaVidaDocenteCRP.Where(x => x.hojavidadocentecrp_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "hojavidadocentecrp_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavidadocentecrp_id)
                                                    : query.OrderBy(x => x.hojavidadocentecrp_id);
                    }
                    if (grilla.columna == "hojavida_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavida_id)
                                                    : query.OrderBy(x => x.hojavida_id);
                    }
                    if (grilla.columna == "certificacion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.certificacion)
                                                    : query.OrderBy(x => x.certificacion);
                    }
                    if (grilla.columna == "institucion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.institucion)
                                                    : query.OrderBy(x => x.institucion);
                    }
                    if (grilla.columna == "año")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.año)
                                                    : query.OrderBy(x => x.año);
                    }
                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "certificacion")
                            query = query.Where(x => x.certificacion.StartsWith(f.valor));
                    }

                    var rango = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in rango
                            select new
                            {
                                m.hojavidadocentecrp_id,
                                m.certificacion,
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
        public HojaVidaDocenteCRP Obtener(int id)//retornar un objeto
        {
            var rango = new HojaVidaDocenteCRP();
            try
            {
                using (var db = new ModeloDatos())
                {
                    rango = db.HojaVidaDocenteCRP
                        .Where(x => x.hojavidadocentecrp_id == id)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return rango;
        }
        //metodo buscar falta aun
        /* public List<Rango> Buscar(string criterio)//retornar un objeto
         {
             var rango = new List<Cargo>();
             String estado = "";
             if (criterio == "Activo") estado = "A";
             if (criterio == "Inactivo") estado = "I";
             try
             {
                 using (var db = new db_proyecto())
                 {
                    // rango = db.Rango
                            // .Where(x => x.nomb_car.Contains(criterio) || x.nomb_car == estado)
                            // .ToList();
                 }
             }
             catch (Exception ex)
             {
                 throw;
             }
             return rango;
         }*/
        //METODO GUARDAR
        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.hojavidadocentecrp_id > 0)
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
