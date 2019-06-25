namespace SistemaPortafolio.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModeloDatos : DbContext
    {
        public ModeloDatos()
             : base("name=ModeloDatos")
        {
        }

        public virtual DbSet<Ciclo> Ciclo { get; set; }
        public virtual DbSet<ConfigEntrega> ConfigEntrega { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<CursoAlumno> CursoAlumno { get; set; }
        public virtual DbSet<CursoDocente> CursoDocente { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<HojaVida> HojaVida { get; set; }
        public virtual DbSet<HojaVidaDocenteCRP> HojaVidaDocenteCRP { get; set; }
        public virtual DbSet<HojaVidaDocenteEX> HojaVidaDocenteEX { get; set; }
        public virtual DbSet<HojaVidaDocenteFA> HojaVidaDocenteFA { get; set; }
        public virtual DbSet<HojaVidaDocenteFC> HojaVidaDocenteFC { get; set; }
        public virtual DbSet<MetadataDocumento> MetadataDocumento { get; set; }
        public virtual DbSet<Notificacion> Notificacion { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PlanEstudio> PlanEstudio { get; set; }
        public virtual DbSet<Semestre> Semestre { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoPersona> TipoPersona { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Unidad> Unidad { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ciclo>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Ciclo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Ciclo>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Ciclo>()
                .HasMany(e => e.Curso)
                .WithRequired(e => e.Ciclo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConfigEntrega>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<ConfigEntrega>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.curso_cod)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.prerequisito)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.CursoAlumno)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.CursoDocente)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Documento)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.MetadataDocumento)
                .WithRequired(e => e.Curso)
                .HasForeignKey(e => e.cod_curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CursoAlumno>()
                .Property(e => e.curso_cod)
                .IsUnicode(false);

            modelBuilder.Entity<CursoDocente>()
                .Property(e => e.curso_cod)
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.curso_cod)
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.archivo)
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.extension)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.tamanio)
                .IsUnicode(false);

            modelBuilder.Entity<Documento>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVida>()
                .HasMany(e => e.HojaVidaDocenteFA)
                .WithRequired(e => e.HojaVida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HojaVida>()
                .HasMany(e => e.HojaVidaDocenteFC)
                .WithRequired(e => e.HojaVida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HojaVida>()
                .HasMany(e => e.HojaVidaDocenteCRP)
                .WithRequired(e => e.HojaVida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HojaVida>()
                .HasMany(e => e.HojaVidaDocenteEX)
                .WithRequired(e => e.HojaVida)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HojaVidaDocenteCRP>()
                .Property(e => e.certificacion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteCRP>()
                .Property(e => e.institucion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteCRP>()
                .Property(e => e.año)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteEX>()
                .Property(e => e.institucion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteEX>()
                .Property(e => e.funcion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFA>()
                .Property(e => e.institucion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFA>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFC>()
                .Property(e => e.institucion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFC>()
                .Property(e => e.tiempo_duracion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFC>()
                .Property(e => e.duracion)
                .IsUnicode(false);

            modelBuilder.Entity<HojaVidaDocenteFC>()
                .Property(e => e.curso)
                .IsUnicode(false);

            modelBuilder.Entity<MetadataDocumento>()
                .Property(e => e.cod_curso)
                .IsUnicode(false);

            modelBuilder.Entity<MetadataDocumento>()
                .Property(e => e.tamanio)
                .IsUnicode(false);

            modelBuilder.Entity<MetadataDocumento>()
                .Property(e => e.programa_nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Notificacion>()
                .Property(e => e.titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Notificacion>()
                .Property(e => e.asunto)
                .IsUnicode(false);

            modelBuilder.Entity<Notificacion>()
                .Property(e => e.mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<Notificacion>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.dni)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.codigo)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.CursoAlumno)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.CursoDocente)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.Documento)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.HojaVida)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.MetadataDocumento)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.Notificacion)
                .WithRequired(e => e.Persona)
                .HasForeignKey(e => e.persona_emisor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.Notificacion1)
                .WithRequired(e => e.Persona1)
                .HasForeignKey(e => e.persona_receptor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Persona>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Persona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanEstudio>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<PlanEstudio>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<PlanEstudio>()
                .HasMany(e => e.Curso)
                .WithRequired(e => e.PlanEstudio)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semestre>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Semestre>()
                .Property(e => e.anio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Semestre>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Semestre>()
                .HasMany(e => e.MetadataDocumento)
                .WithRequired(e => e.Semestre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semestre>()
                .HasMany(e => e.Unidad)
                .WithRequired(e => e.Semestre)
                .HasForeignKey(e => e.id_semestre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoDocumento>()
                .Property(e => e.extension)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumento>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumento>()
                .HasMany(e => e.Documento)
                .WithRequired(e => e.TipoDocumento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoDocumento>()
                .HasMany(e => e.MetadataDocumento)
                .WithRequired(e => e.TipoDocumento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoPersona>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoPersona>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<TipoPersona>()
                .HasMany(e => e.Persona)
                .WithRequired(e => e.TipoPersona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoPersona>()
                .HasMany(e => e.TipoDocumento)
                .WithRequired(e => e.TipoPersona)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoUsuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<TipoUsuario>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<TipoUsuario>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<TipoUsuario>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.TipoUsuario1)
                .HasForeignKey(e => e.tipousuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unidad>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Unidad>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<Unidad>()
                .HasMany(e => e.ConfigEntrega)
                .WithRequired(e => e.Unidad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.estado)
                .IsUnicode(false);
        }
    }
}
