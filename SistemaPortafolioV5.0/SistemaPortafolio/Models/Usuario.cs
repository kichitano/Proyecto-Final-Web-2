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
    using System.IO;
    using System.Web;
    using System.Data.Entity.Validation;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        public int usuario_id { get; set; }

        public int persona_id { get; set; }

        public int tipousuario { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string clave { get; set; }

        [Required]
        [StringLength(200)]
        public string avatar { get; set; }

        [Required]
        [StringLength(10)]
        public string estado { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual TipoUsuario TipoUsuario1 { get; set; }

        public List<Usuario> Listar()
        {
            var usuario = new List<Usuario>();
            try
            {
                using (var db = new ModeloDatos())
                {
                    //usuario = db.Usuario.ToList();
                    usuario = db.Usuario.Include("persona").ToList();
                    usuario = db.Usuario.Include("tipousuario").ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return usuario;
        }

        //para la anexgrid
        public AnexGRIDResponde ListarGrilla(AnexGRID grilla)
        {
            try
            {
                using (var db = new ModeloDatos())
                {

                    grilla.Inicializar();
                    var query = db.Usuario.Include("Persona").Include("TipoUsuario1").Where(x => x.persona_id > 0).Where(x => x.tipousuario > 0);

                    //ordenar las columnas a mostrar

                    if (grilla.columna == "usuario_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuario_id)
                                                                : query.OrderBy(x => x.usuario_id);
                    }
                    if (grilla.columna == "persona_id")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.persona_id)
                                                                : query.OrderBy(x => x.persona_id);
                    }
                    if (grilla.columna == "tipousuario")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipousuario)
                                                                : query.OrderBy(x => x.tipousuario);
                    }
                    if (grilla.columna == "nombre")
                    {
                        query = grilla.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre)
                                                                : query.OrderBy(x => x.nombre);
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


                    var usuario = query.Skip(grilla.pagina)
                                        .Take(grilla.limite)
                                        .ToList();
                    var total = query.Count();
                    grilla.SetData(
                        from s in usuario
                        select new
                        {
                            s.usuario_id,
                            nombrepersona = s.Persona.nombre,
                            nombretipousuario = s.TipoUsuario1.nombre,
                            s.nombre,
                            s.estado

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

        //metodo buscar
        public List<Usuario> Buscar(string criterio)//retornar un objeto
        {
            var usuario = new List<Usuario>();
            String estado = "";
            if (criterio == "Activo") estado = "Activo";
            if (criterio == "Inactivo") estado = "Inactivo";
            try
            {
                using (var db = new ModeloDatos())
                {
                    usuario = db.Usuario
                            .Where(x => x.nombre.Contains(criterio) || x.estado == estado)
                            .ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return usuario;
        }
        //METODO GUARDAR
        public void Guardar()
        {
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Configuration.ValidateOnSaveEnabled = false;

                    //this.semestre_id es como un "boolean"
                    if (this.usuario_id > 0)
                    {
                        db.Entry(this).State = EntityState.Modified;

                        if (this.clave != null)
                        {
                            this.clave = HashHelper.MD5(this.clave);
                        }
                        else
                        {
                            db.Entry(this).Property(x => x.clave).IsModified = false;
                        }
                    }
                    else
                    {
                        this.clave = HashHelper.MD5(this.clave);
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



        public List<Usuario> listarusuarios()
        {
            var usuario = new List<Usuario>();

            try
            {
                using (var db = new ModeloDatos())
                {
                    usuario = db.Usuario.Include("Persona").Include("TipoUsuario1").ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }

        public Usuario Obtener(int id)
        {
            var usuario = new Usuario();

            try
            {
                using (var db = new ModeloDatos())
                {
                    usuario = db.Usuario.Include("Persona").Include(x => x.Persona.TipoPersona).Include("TipoUsuario1")
                    .Where(x => x.usuario_id == id)
                    .SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }

        //int numero_intentos = 1;

        //validar login
        public ResponseModel ValidarLogin(string Usuario, string Password)
        {
            //if (HttpContext.Current.Session["numero_intentos"] != null)
            //{
            //    numero_intentos = Int32.Parse(HttpContext.Current.Session["numero_intentos"].ToString());
            //}


            var rm = new ResponseModel();
            try
            {
                using (var db = new ModeloDatos())
                {
                    Password = HashHelper.MD5(Password);

                    var usuario = db.Usuario.Include("TipoUsuario1").Where(x => x.nombre == Usuario).SingleOrDefault();

                    if (usuario != null)
                    {
                        //if (numero_intentos == 3)
                        //{
                        //    rm.SetResponse(false, "Usuario bloqueado por numero de intentos fallidos.");

                        //    var u = new Usuario();

                        //    u.usuario_id = usuario.usuario_id;
                        //    u.persona_id = usuario.persona_id;
                        //    u.tipousuario = usuario.tipousuario;
                        //    u.nombre = usuario.nombre;
                        //    u.clave = usuario.clave;
                        //    u.avatar = usuario.avatar;
                        //    u.estado = "Inactivo";
                        //    u.Guardar();
                        //}
                        if(usuario.estado.Equals("Inactivo"))
                        {
                            rm.SetResponse(false, "¡El usuario ingresado esta en estado: INACTIVO!");
                        }
                        else if (usuario.clave == Password)
                        {
                            SessionHelper.AddUserToSession(usuario.usuario_id.ToString());
                            rm.message = "";
                            if (usuario.TipoUsuario1.nombre.Equals("Administrador"))
                            {
                                rm.SetResponse(true,"admin");
                            }
                            if (usuario.TipoUsuario1.nombre.Equals("Usuario"))
                            {
                                rm.SetResponse(true,"user");
                            }
                        }
                        else
                        {
                            rm.SetResponse(false, "¡Datos del usuario incorrectos!");
                        }
                        //else
                        //{
                        //    rm.SetResponse(false, "¡password invalido! Le quedan " + (3 - numero_intentos) + " intentos.");
                        //    numero_intentos++;
                        //}
                    }
                    else
                    {
                        rm.SetResponse(false, "¡Usuario no existe!");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


            //HttpContext.Current.Session["numero_intentos"] = numero_intentos;

            return rm;
        }
        public ResponseModel GuardarPerfil(HttpPostedFileBase Foto)
        {
            var rm = new ResponseModel();
            try
            {
                using (var db = new ModeloDatos())
                {
                    db.Configuration.ValidateOnSaveEnabled = false;

                    var Usu = db.Entry(this);

                    Usu.State = EntityState.Modified;

                    if (Foto != null)
                    {
                        string extension = Path.GetExtension(Foto.FileName).ToLower();
                        int size = 1024 * 1024 * 7; //7 megas
                        var filtroextension = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extensiones = Path.GetExtension(Foto.FileName);

                        if (filtroextension.Contains(extensiones) && (Foto.ContentLength < size))
                        {
                            string archivo = Path.GetFileName(Foto.FileName);
                            Foto.SaveAs(HttpContext.Current.Server.MapPath("~/Server/Images/" + archivo));
                            this.avatar = archivo;
                        }

                    }
                    else Usu.Property(x => x.avatar).IsModified = false;

                    if(this.clave == null)
                    {
                        Usu.Property(x => x.clave).IsModified = false;
                    }
                    else
                    {
                        this.clave = HashHelper.MD5(this.clave);
                    }
                    if (this.usuario_id == 0) Usu.Property(x => x.usuario_id).IsModified = false;
                    if (this.persona_id == 0) Usu.Property(x => x.persona_id).IsModified = false;
                    if (this.tipousuario == 0) Usu.Property(x => x.tipousuario).IsModified = false;
                    if (this.estado == null) Usu.Property(x => x.estado).IsModified = false;
                    db.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return rm;
        }

        //grafico01
        public string[,] ConsultaTotalUsuarios()
        {
            string[,] usuarios;
            try
            {
                using (var db = new ModeloDatos())
                {
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
