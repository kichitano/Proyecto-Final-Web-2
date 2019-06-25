namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.Entity;
    using System.Linq;

    [Table("HojaVida")]
    public partial class HojaVida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HojaVida()
        {
            HojaVidaDocenteFA = new HashSet<HojaVidaDocenteFA>();
            HojaVidaDocenteFC = new HashSet<HojaVidaDocenteFC>();
            HojaVidaDocenteCRP = new HashSet<HojaVidaDocenteCRP>();
            HojaVidaDocenteEX = new HashSet<HojaVidaDocenteEX>();
        }

        [Key]
        public int hojavida_id { get; set; }

        public int persona_id { get; set; }

        public virtual Persona Persona { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDocenteFA> HojaVidaDocenteFA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDocenteFC> HojaVidaDocenteFC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDocenteCRP> HojaVidaDocenteCRP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDocenteEX> HojaVidaDocenteEX { get; set; }

        public List<HojaVida> Listar()
        {
            var rango = new List<HojaVida>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    rango = db.HojaVida.Include("Persona").ToList();
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
                    var query = db.HojaVida.Where(x => x.hojavida_id > 0);
                    //obtener los campos y que permita ordenar
                    if (grilla.columna == "hojavida_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.hojavida_id)
                                                    : query.OrderBy(x => x.hojavida_id);
                    }
                    if (grilla.columna == "persona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.persona_id)
                                                    : query.OrderBy(x => x.persona_id);
                    }
                    // Filtrar
                    foreach (var f in grilla.filtros)
                    {
                        if (f.columna == "hojavida_id")
                            query = query.Where(x => x.hojavida_id.ToString().StartsWith(f.valor));
                    }

                    var rango = query.Skip(grilla.pagina)
                                    .Take(grilla.limite)
                                    .ToList();
                    var total = query.Count();//cantidad de registros

                    grilla.SetData(
                            from m in rango
                            select new
                            {
                                m.hojavida_id,
                                m.persona_id,
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
        public HojaVida Obtener(int id)//retornar un objeto
        {
            var rango = new HojaVida();
            try
            {
                using (var db = new ModeloDatos())
                {
                    rango = db.HojaVida.Include("persona")
                        .Where(x => x.hojavida_id == id)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return rango;
        }

        public HojaVida ObtenerByPersona(int persona_id)//retornar un objeto
        {
            var rango = new HojaVida();
            try
            {
                using (var db = new ModeloDatos())
                {
                    rango = db.HojaVida.Include("persona")
                        .Where(x => x.persona_id == persona_id)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
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
                    if (this.hojavida_id > 0)
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
        public string[,] Consulta()
        {
            string[,] criterios;
            try
            {
                using (var db = new ModeloDatos())
                {
                    var per = (from m in db.HojaVida
                               join c in db.Persona on
                               m.persona_id equals c.persona_id into r_join
                               from c in r_join.DefaultIfEmpty()
                               group new { m, c } by new
                               {
                                   m.hojavida_id,
                                   m.persona_id
                               } into g
                               select new
                               {
                                   id_persona = (int?)g.Key.hojavida_id,
                                   g.Key.hojavida_id,
                                   Cantidad = g.Count(p => p.c.persona_id != null)
                               }).ToList();
                    criterios = new string[per.Count(), 2];
                    int count = 0;
                    foreach (var m in per)
                    {
                        criterios[count, 0] = Convert.ToString(m.hojavida_id);
                        criterios[count, 1] = Convert.ToString(m.Cantidad);
                        count++;
                    }
                }
                return criterios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[,] ConsultaTotalExperiencia()
        {
            string[,] usuarios;
            try
            {
                using (var db = new ModeloDatos())
                {
                    //var query = (from x in db.HojaVidaDocenteCRP where x.HojaVida.Persona.nombre == "Docente" select new { id = x.hojavidadocentecrp_id, docente = x.HojaVida.Persona.nombre });
                    //.Concat(from y in db.HojaVidaDocenteEX where y.HojaVida.Persona.nombre == "Docente" select new { id = y.hojavidadocenteex_id, docente = y.HojaVida.Persona.nombre })
                    //.Concat(from z in db.HojaVidaDocenteFA where z.HojaVida.Persona.nombre == "Docente" select new { id = z.hojavidadocentefa_id, docente = z.HojaVida.Persona.nombre })
                    //.Concat(from w in db.HojaVidaDocenteFC where w.HojaVida.Persona.nombre == "Docente" select new { id = w.hojavidadocentefc_id, docente = w.HojaVida.Persona.nombre });

                    var q1 = db.HojaVidaDocenteCRP.Include("HojaVida").Include("HojaVida.Persona").Include("HojaVida.Persona.TipoPersona").Where(x => x.HojaVida.Persona.TipoPersona.nombre.Equals("Docente")).Count();
                    var q2 = db.HojaVidaDocenteEX.Include("HojaVida").Include("HojaVida.Persona").Include("HojaVida.Persona.TipoPersona").Where(x => x.HojaVida.Persona.TipoPersona.nombre.Equals("Docente")).Count();
                    var q3 = db.HojaVidaDocenteFA.Include("HojaVida").Include("HojaVida.Persona").Include("HojaVida.Persona.TipoPersona").Where(x => x.HojaVida.Persona.TipoPersona.nombre.Equals("Docente")).Count();
                    var q4 = db.HojaVidaDocenteFC.Include("HojaVida").Include("HojaVida.Persona").Include("HojaVida.Persona.TipoPersona").Where(x => x.HojaVida.Persona.TipoPersona.nombre.Equals("Docente")).Count();
                    
                    usuarios = new string[4, 2];

                    usuarios[0, 0] = "HojaVidaDocenteCRP";
                    usuarios[0, 1] = q1 + "";

                    usuarios[1, 0] = "HojaVidaDocenteEX";
                    usuarios[1, 1] = q2 + "";

                    usuarios[2, 0] = "HojaVidaDocenteFA";
                    usuarios[2, 1] = q3 + "";

                    usuarios[3, 0] = "HojaVidaDocenteFC";
                    usuarios[3, 1] = q4 + "";

                    /*
                    var con = (from p in db.TipoUsuario
                               join c in db.Usuario on p.tipousuario_id equals c.tipousuario into r_join
                               from c in r_join.DefaultIfEmpty()
                               group new { p, c } by new
                               {
                                   p.tipousuario_id,
                                   p.nombre
                               } into g
                               select new
                               {
                                   id_curso = (int?)g.Key.tipousuario_id,
                                   g.Key.nombre,
                                   Cantidad = g.Count(p => p.c.tipousuario != 0)
                               }).ToList();

                    usuarios = new string[con.Count(), 2];
                    int count = 0;
                    foreach (var p in con)
                    {
                        usuarios[count, 0] = Convert.ToString(p.nombre);
                        usuarios[count, 1] = Convert.ToString(p.Cantidad);
                        count++;
                    }
                    */
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
