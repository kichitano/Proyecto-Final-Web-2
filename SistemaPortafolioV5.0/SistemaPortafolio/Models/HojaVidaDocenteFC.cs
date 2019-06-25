namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;

    [Table("HojaVidaDocenteFC")]
    public partial class HojaVidaDocenteFC
    {
        [Key]
        public int hojavidadocentefc_id { get; set; }

        public int hojavida_id { get; set; }

        [Required]
        [StringLength(150)]
        public string institucion { get; set; }

        [Required]
        [StringLength(150)]
        public string tiempo_duracion { get; set; }

        [Required]
        [StringLength(150)]
        public string duracion { get; set; }

        [Required]
        [StringLength(150)]
        public string curso { get; set; }

        public virtual HojaVida HojaVida { get; set; }

        public List<HojaVidaDocenteFC> Listar(int codigo)
        {
            var persona = new List<HojaVidaDocenteFC>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFC.Where(x => x.HojaVida.persona_id == codigo).ToList();
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
                    var query = db.HojaVidaDocenteFC.Where(x => x.hojavidadocentefc_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "hojavidadocentefc_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavidadocentefc_id)
                                                    : query.OrderBy(x => x.hojavidadocentefc_id);
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
                    if (grilla.columna == "tiempo_duracion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tiempo_duracion)
                                                    : query.OrderBy(x => x.tiempo_duracion);
                    }
                    if (grilla.columna == "duracion")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.duracion)
                                                    : query.OrderBy(x => x.duracion);
                    }
                    if (grilla.columna == "curso")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.curso)
                                                    : query.OrderBy(x => x.curso);
                    }

                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "institucion")
                            query = query.Where(x => x.institucion.StartsWith(f.valor));
                        if (f.columna == "tiempo_duracion")
                            query = query.Where(x => x.tiempo_duracion.StartsWith(f.valor));
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.hojavidadocentefc_id,
                                m.hojavida_id,
                                m.institucion,
                                m.tiempo_duracion,
                                m.duracion,
                                m.curso,
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
        public HojaVidaDocenteFC Obtener(int id)//retornar un objeto
        {
            var persona = new HojaVidaDocenteFC();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFC
                        .Where(x => x.hojavidadocentefc_id == id)
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
        public List<HojaVidaDocenteFC> Buscar(string criterio)//retornar un objeto
        {
            var persona = new List<HojaVidaDocenteFC>();
            //  String estado = "";
            //   if (criterio == "Activo") estado = "Activo";
            //   if (criterio == "Inactivo") estado = "Inactivo";
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFC
                            .Where(x => x.institucion.Contains(criterio))
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
                    if (this.hojavidadocentefc_id > 0)
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
        //consulta una Persona cuantos usuarios tiene 
    }
}
