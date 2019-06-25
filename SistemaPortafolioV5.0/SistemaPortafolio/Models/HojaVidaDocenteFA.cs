namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;

    [Table("HojaVidaDocenteFA")]
    public partial class HojaVidaDocenteFA
    {
        [Key]
        public int hojavidadocentefa_id { get; set; }

        public int hojavida_id { get; set; }

        [Required]
        [StringLength(150)]
        public string institucion { get; set; }

        [Required]
        [StringLength(150)]
        public string titulo { get; set; }

        public virtual HojaVida HojaVida { get; set; }

        public List<HojaVidaDocenteFA> Listar(int codigo)
        {
            var persona = new List<HojaVidaDocenteFA>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFA.Where(x => x.HojaVida.persona_id == codigo).ToList();
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
                    var query = db.HojaVidaDocenteFA.Where(x => x.hojavidadocentefa_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "hojavidadocentefa_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavidadocentefa_id)
                                                    : query.OrderBy(x => x.hojavidadocentefa_id);
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
                    if (grilla.columna == "titulo")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.titulo)
                                                    : query.OrderBy(x => x.titulo);
                    }


                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "institucion")
                            query = query.Where(x => x.institucion.StartsWith(f.valor));
                        if (f.columna == "titulo")
                            query = query.Where(x => x.titulo.StartsWith(f.valor));
                    }

                    var cargo = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in cargo
                            select new
                            {
                                m.hojavidadocentefa_id,
                                m.hojavida_id,
                                m.titulo,
                                m.institucion,

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
        public HojaVidaDocenteFA Obtener(int id)//retornar un objeto
        {
            var persona = new HojaVidaDocenteFA();
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFA
                        .Where(x => x.hojavidadocentefa_id == id)
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
        public List<HojaVidaDocenteFA> Buscar(string criterio)//retornar un objeto
        {
            var persona = new List<HojaVidaDocenteFA>();
            //   String estado = "";
            //    if (criterio == "Activo") estado = "Activo";
            //   if (criterio == "Inactivo") estado = "Inactivo";
            try
            {
                using (var db = new ModeloDatos())
                {
                    persona = db.HojaVidaDocenteFA
                            .Where(x => x.titulo.Contains(criterio) || x.institucion == institucion)
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
                    if (this.hojavidadocentefa_id > 0)
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
