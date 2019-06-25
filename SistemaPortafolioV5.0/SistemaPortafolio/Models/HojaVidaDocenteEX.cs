namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.Entity;
    using System.Linq;

    [Table("HojaVidaDocenteEX")]
    public partial class HojaVidaDocenteEX
    {
        [Key]
        public int hojavidadocenteex_id { get; set; }

        public int hojavida_id { get; set; }

        [Required]
        [StringLength(150)]
        public string institucion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fechainicio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fechafin { get; set; }

        [Column(TypeName = "text")]
        public string funcion { get; set; }

        public virtual HojaVida HojaVida { get; set; }

        public List<HojaVidaDocenteEX> Listar(int codigo)
        {
            var persona = new List<HojaVidaDocenteEX>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteEX.Where(x => x.HojaVida.persona_id == codigo).ToList();
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
                    var query = db.HojaVidaDocenteEX.Where(x => x.hojavidadocenteex_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "hojavidadocenteex_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavidadocenteex_id)
                                                    : query.OrderBy(x => x.hojavidadocenteex_id);
                    }
                    if (grilla.columna == "hojavida_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavida_id)
                                                    : query.OrderBy(x => x.hojavida_id);
                    }
                    if (grilla.columna == "institucion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.institucion)
                                                    : query.OrderBy(x => x.institucion);
                    }
                    if (grilla.columna == "fechainicio")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.fechainicio)
                                                    : query.OrderBy(x => x.fechainicio);
                    }
                    if (grilla.columna == "fechafin")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.fechafin)
                                                    : query.OrderBy(x => x.fechafin);
                    }
                    if (grilla.columna == "funcion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.funcion)
                                                    : query.OrderBy(x => x.funcion);
                    }
                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "institucion")
                            query = query.Where(x => x.institucion.StartsWith(f.valor));
                        if (f.columna == "funcion")
                            query = query.Where(x => x.funcion.StartsWith(f.valor));
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.hojavidadocenteex_id,
                                m.hojavida_id,
                                m.institucion,
                                m.fechainicio,
                                m.fechafin,
                                m.funcion,
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
        public HojaVidaDocenteEX Obtener(int id)//retornar un objeto
        {
            var persona = new HojaVidaDocenteEX();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteEX
                        .Where(x => x.hojavidadocenteex_id == id)
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
        public List<HojaVidaDocenteEX> Buscar(string criterio)//retornar un objeto
        {
            var persona = new List<HojaVidaDocenteEX>();
            //   String estado = "";
            //    if (criterio == "Activo") estado = "Activo";
            //   if (criterio == "Inactivo") estado = "Inactivo";
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteEX
                            .Where(x => x.institucion.Contains(criterio) || x.fechainicio == fechainicio)
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
        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    if (this.hojavidadocenteex_id > 0)
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
