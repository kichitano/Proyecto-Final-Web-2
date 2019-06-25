namespace SistemaPortafolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ciclo",
                c => new
                    {
                        ciclo_id = c.Int(nullable: false, identity: true),
                        nombre = c.String(maxLength: 100, unicode: false),
                        descripcion = c.String(unicode: false, storeType: "text"),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.ciclo_id);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        curso_cod = c.String(nullable: false, maxLength: 100, unicode: false),
                        plan_id = c.Int(nullable: false),
                        ciclo_id = c.Int(nullable: false),
                        nombre = c.String(nullable: false, maxLength: 150, unicode: false),
                        credito = c.Int(nullable: false),
                        horasteoria = c.Int(nullable: false),
                        horaspractica = c.Int(nullable: false),
                        totalhoras = c.Int(nullable: false),
                        prerequisito = c.String(maxLength: 150, unicode: false),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.curso_cod)
                .ForeignKey("dbo.PlanEstudio", t => t.plan_id)
                .ForeignKey("dbo.Ciclo", t => t.ciclo_id)
                .Index(t => t.plan_id)
                .Index(t => t.ciclo_id);
            
            CreateTable(
                "dbo.CursoAlumno",
                c => new
                    {
                        cursoalumno_id = c.Int(nullable: false, identity: true),
                        curso_cod = c.String(nullable: false, maxLength: 100, unicode: false),
                        persona_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.cursoalumno_id)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .ForeignKey("dbo.Curso", t => t.curso_cod)
                .Index(t => t.curso_cod)
                .Index(t => t.persona_id);
            
            CreateTable(
                "dbo.Persona",
                c => new
                    {
                        persona_id = c.Int(nullable: false, identity: true),
                        tipopersona_id = c.Int(nullable: false),
                        dni = c.String(nullable: false, maxLength: 8, unicode: false),
                        codigo = c.String(maxLength: 10, unicode: false),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        apellido = c.String(nullable: false, maxLength: 100, unicode: false),
                        email = c.String(maxLength: 100, unicode: false),
                        celular = c.String(maxLength: 15, unicode: false),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.persona_id)
                .ForeignKey("dbo.TipoPersona", t => t.tipopersona_id)
                .Index(t => t.tipopersona_id);
            
            CreateTable(
                "dbo.CursoDocente",
                c => new
                    {
                        cursodocente_id = c.Int(nullable: false, identity: true),
                        curso_cod = c.String(nullable: false, maxLength: 100, unicode: false),
                        persona_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.cursodocente_id)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .ForeignKey("dbo.Curso", t => t.curso_cod)
                .Index(t => t.curso_cod)
                .Index(t => t.persona_id);
            
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        documento_id = c.Int(nullable: false, identity: true),
                        tipodocumento_id = c.Int(nullable: false),
                        persona_id = c.Int(nullable: false),
                        id_unidad = c.Int(nullable: false),
                        curso_cod = c.String(nullable: false, maxLength: 100, unicode: false),
                        archivo = c.String(nullable: false, maxLength: 150, unicode: false),
                        descripcion = c.String(nullable: false, unicode: false, storeType: "text"),
                        fecha_entrega = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        hora_entrega = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        extension = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        tamanio = c.String(maxLength: 50, unicode: false),
                        estado = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.documento_id)
                .ForeignKey("dbo.Unidad", t => t.id_unidad, cascadeDelete: true)
                .ForeignKey("dbo.TipoDocumento", t => t.tipodocumento_id)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .ForeignKey("dbo.Curso", t => t.curso_cod)
                .Index(t => t.tipodocumento_id)
                .Index(t => t.persona_id)
                .Index(t => t.id_unidad)
                .Index(t => t.curso_cod);
            
            CreateTable(
                "dbo.MetadataDocumento",
                c => new
                    {
                        metadata_id = c.Int(nullable: false, identity: true),
                        documento_id = c.Int(),
                        cod_curso = c.String(nullable: false, maxLength: 100, unicode: false),
                        persona_id = c.Int(nullable: false),
                        semestre_id = c.Int(nullable: false),
                        tipodocumento_id = c.Int(nullable: false),
                        id_unidad = c.Int(),
                        pagina_total = c.Int(),
                        palabra_total = c.Int(),
                        caracter_total = c.Int(),
                        linea_total = c.Int(),
                        parrafo_total = c.Int(),
                        celda = c.Int(),
                        columna = c.Int(),
                        diapositiva = c.Int(),
                        tamanio = c.String(maxLength: 100, unicode: false),
                        programa_nombre = c.String(maxLength: 100, unicode: false),
                        fecha_creacion = c.DateTime(storeType: "smalldatetime"),
                        fecha_subida = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.metadata_id)
                .ForeignKey("dbo.Documento", t => t.documento_id)
                .ForeignKey("dbo.Semestre", t => t.semestre_id)
                .ForeignKey("dbo.Unidad", t => t.id_unidad)
                .ForeignKey("dbo.TipoDocumento", t => t.tipodocumento_id)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .ForeignKey("dbo.Curso", t => t.cod_curso)
                .Index(t => t.documento_id)
                .Index(t => t.cod_curso)
                .Index(t => t.persona_id)
                .Index(t => t.semestre_id)
                .Index(t => t.tipodocumento_id)
                .Index(t => t.id_unidad);
            
            CreateTable(
                "dbo.Semestre",
                c => new
                    {
                        semestre_id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        anio = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false),
                        fechainicio = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        fechafin = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.semestre_id);
            
            CreateTable(
                "dbo.PlanEstudio",
                c => new
                    {
                        plan_id = c.Int(nullable: false, identity: true),
                        semestre_id = c.Int(),
                        nombre = c.String(maxLength: 100, unicode: false),
                        estado = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.plan_id)
                .ForeignKey("dbo.Semestre", t => t.semestre_id)
                .Index(t => t.semestre_id);
            
            CreateTable(
                "dbo.Unidad",
                c => new
                    {
                        id_unidad = c.Int(nullable: false, identity: true),
                        id_semestre = c.Int(nullable: false),
                        descripcion = c.String(maxLength: 150, unicode: false),
                        estado = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.id_unidad)
                .ForeignKey("dbo.Semestre", t => t.id_semestre)
                .Index(t => t.id_semestre);
            
            CreateTable(
                "dbo.ConfigEntrega",
                c => new
                    {
                        configentrega_id = c.Int(nullable: false, identity: true),
                        id_unidad = c.Int(nullable: false),
                        nombre = c.String(maxLength: 150, unicode: false),
                        fecha_inicio = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        fecha_fin = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        estado = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.configentrega_id)
                .ForeignKey("dbo.Unidad", t => t.id_unidad)
                .Index(t => t.id_unidad);
            
            CreateTable(
                "dbo.TipoDocumento",
                c => new
                    {
                        tipodocumento_id = c.Int(nullable: false, identity: true),
                        tipopersona_id = c.Int(nullable: false),
                        nombre = c.String(nullable: false, maxLength: 150, unicode: false),
                        extension = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.tipodocumento_id)
                .ForeignKey("dbo.TipoPersona", t => t.tipopersona_id)
                .Index(t => t.tipopersona_id);
            
            CreateTable(
                "dbo.TipoPersona",
                c => new
                    {
                        tipopersona_id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.tipopersona_id);
            
            CreateTable(
                "dbo.HojaVida",
                c => new
                    {
                        hojavida_id = c.Int(nullable: false, identity: true),
                        persona_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.hojavida_id)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .Index(t => t.persona_id);
            
            CreateTable(
                "dbo.HojaVidaDocenteCRP",
                c => new
                    {
                        hojavidadocentecrp_id = c.Int(nullable: false, identity: true),
                        hojavida_id = c.Int(nullable: false),
                        certificacion = c.String(nullable: false, maxLength: 150, unicode: false),
                        institucion = c.String(nullable: false, maxLength: 150, unicode: false),
                        año = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.hojavidadocentecrp_id)
                .ForeignKey("dbo.HojaVida", t => t.hojavida_id)
                .Index(t => t.hojavida_id);
            
            CreateTable(
                "dbo.HojaVidaDocenteEX",
                c => new
                    {
                        hojavidadocenteex_id = c.Int(nullable: false, identity: true),
                        hojavida_id = c.Int(nullable: false),
                        institucion = c.String(nullable: false, maxLength: 150, unicode: false),
                        fechainicio = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        fechafin = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        funcion = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.hojavidadocenteex_id)
                .ForeignKey("dbo.HojaVida", t => t.hojavida_id)
                .Index(t => t.hojavida_id);
            
            CreateTable(
                "dbo.HojaVidaDocenteFA",
                c => new
                    {
                        hojavidadocentefa_id = c.Int(nullable: false, identity: true),
                        hojavida_id = c.Int(nullable: false),
                        institucion = c.String(nullable: false, maxLength: 150, unicode: false),
                        titulo = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.hojavidadocentefa_id)
                .ForeignKey("dbo.HojaVida", t => t.hojavida_id)
                .Index(t => t.hojavida_id);
            
            CreateTable(
                "dbo.HojaVidaDocenteFC",
                c => new
                    {
                        hojavidadocentefc_id = c.Int(nullable: false, identity: true),
                        hojavida_id = c.Int(nullable: false),
                        institucion = c.String(nullable: false, maxLength: 150, unicode: false),
                        tiempo_duracion = c.String(nullable: false, maxLength: 150, unicode: false),
                        duracion = c.String(nullable: false, maxLength: 150, unicode: false),
                        curso = c.String(nullable: false, maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.hojavidadocentefc_id)
                .ForeignKey("dbo.HojaVida", t => t.hojavida_id)
                .Index(t => t.hojavida_id);
            
            CreateTable(
                "dbo.Notificacion",
                c => new
                    {
                        notificacion_id = c.Int(nullable: false, identity: true),
                        persona_emisor = c.Int(nullable: false),
                        persona_receptor = c.Int(nullable: false),
                        titulo = c.String(nullable: false, maxLength: 150, unicode: false),
                        asunto = c.String(nullable: false, maxLength: 150, unicode: false),
                        mensaje = c.String(nullable: false, maxLength: 150, unicode: false),
                        fecha_emision = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.notificacion_id)
                .ForeignKey("dbo.Persona", t => t.persona_emisor)
                .ForeignKey("dbo.Persona", t => t.persona_receptor)
                .Index(t => t.persona_emisor)
                .Index(t => t.persona_receptor);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        usuario_id = c.Int(nullable: false, identity: true),
                        persona_id = c.Int(nullable: false),
                        tipousuario = c.Int(nullable: false),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        clave = c.String(nullable: false, maxLength: 100, unicode: false),
                        avatar = c.String(nullable: false, maxLength: 200, unicode: false),
                        estado = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.usuario_id)
                .ForeignKey("dbo.TipoUsuario", t => t.tipousuario)
                .ForeignKey("dbo.Persona", t => t.persona_id)
                .Index(t => t.persona_id)
                .Index(t => t.tipousuario);
            
            CreateTable(
                "dbo.TipoUsuario",
                c => new
                    {
                        tipousuario_id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        descripcion = c.String(unicode: false, storeType: "text"),
                        estado = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.tipousuario_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Curso", "ciclo_id", "dbo.Ciclo");
            DropForeignKey("dbo.MetadataDocumento", "cod_curso", "dbo.Curso");
            DropForeignKey("dbo.Documento", "curso_cod", "dbo.Curso");
            DropForeignKey("dbo.CursoDocente", "curso_cod", "dbo.Curso");
            DropForeignKey("dbo.CursoAlumno", "curso_cod", "dbo.Curso");
            DropForeignKey("dbo.Usuario", "persona_id", "dbo.Persona");
            DropForeignKey("dbo.Usuario", "tipousuario", "dbo.TipoUsuario");
            DropForeignKey("dbo.Notificacion", "persona_receptor", "dbo.Persona");
            DropForeignKey("dbo.Notificacion", "persona_emisor", "dbo.Persona");
            DropForeignKey("dbo.MetadataDocumento", "persona_id", "dbo.Persona");
            DropForeignKey("dbo.HojaVida", "persona_id", "dbo.Persona");
            DropForeignKey("dbo.HojaVidaDocenteFC", "hojavida_id", "dbo.HojaVida");
            DropForeignKey("dbo.HojaVidaDocenteFA", "hojavida_id", "dbo.HojaVida");
            DropForeignKey("dbo.HojaVidaDocenteEX", "hojavida_id", "dbo.HojaVida");
            DropForeignKey("dbo.HojaVidaDocenteCRP", "hojavida_id", "dbo.HojaVida");
            DropForeignKey("dbo.Documento", "persona_id", "dbo.Persona");
            DropForeignKey("dbo.TipoDocumento", "tipopersona_id", "dbo.TipoPersona");
            DropForeignKey("dbo.Persona", "tipopersona_id", "dbo.TipoPersona");
            DropForeignKey("dbo.MetadataDocumento", "tipodocumento_id", "dbo.TipoDocumento");
            DropForeignKey("dbo.Documento", "tipodocumento_id", "dbo.TipoDocumento");
            DropForeignKey("dbo.Unidad", "id_semestre", "dbo.Semestre");
            DropForeignKey("dbo.MetadataDocumento", "id_unidad", "dbo.Unidad");
            DropForeignKey("dbo.Documento", "id_unidad", "dbo.Unidad");
            DropForeignKey("dbo.ConfigEntrega", "id_unidad", "dbo.Unidad");
            DropForeignKey("dbo.PlanEstudio", "semestre_id", "dbo.Semestre");
            DropForeignKey("dbo.Curso", "plan_id", "dbo.PlanEstudio");
            DropForeignKey("dbo.MetadataDocumento", "semestre_id", "dbo.Semestre");
            DropForeignKey("dbo.MetadataDocumento", "documento_id", "dbo.Documento");
            DropForeignKey("dbo.CursoDocente", "persona_id", "dbo.Persona");
            DropForeignKey("dbo.CursoAlumno", "persona_id", "dbo.Persona");
            DropIndex("dbo.Usuario", new[] { "tipousuario" });
            DropIndex("dbo.Usuario", new[] { "persona_id" });
            DropIndex("dbo.Notificacion", new[] { "persona_receptor" });
            DropIndex("dbo.Notificacion", new[] { "persona_emisor" });
            DropIndex("dbo.HojaVidaDocenteFC", new[] { "hojavida_id" });
            DropIndex("dbo.HojaVidaDocenteFA", new[] { "hojavida_id" });
            DropIndex("dbo.HojaVidaDocenteEX", new[] { "hojavida_id" });
            DropIndex("dbo.HojaVidaDocenteCRP", new[] { "hojavida_id" });
            DropIndex("dbo.HojaVida", new[] { "persona_id" });
            DropIndex("dbo.TipoDocumento", new[] { "tipopersona_id" });
            DropIndex("dbo.ConfigEntrega", new[] { "id_unidad" });
            DropIndex("dbo.Unidad", new[] { "id_semestre" });
            DropIndex("dbo.PlanEstudio", new[] { "semestre_id" });
            DropIndex("dbo.MetadataDocumento", new[] { "id_unidad" });
            DropIndex("dbo.MetadataDocumento", new[] { "tipodocumento_id" });
            DropIndex("dbo.MetadataDocumento", new[] { "semestre_id" });
            DropIndex("dbo.MetadataDocumento", new[] { "persona_id" });
            DropIndex("dbo.MetadataDocumento", new[] { "cod_curso" });
            DropIndex("dbo.MetadataDocumento", new[] { "documento_id" });
            DropIndex("dbo.Documento", new[] { "curso_cod" });
            DropIndex("dbo.Documento", new[] { "id_unidad" });
            DropIndex("dbo.Documento", new[] { "persona_id" });
            DropIndex("dbo.Documento", new[] { "tipodocumento_id" });
            DropIndex("dbo.CursoDocente", new[] { "persona_id" });
            DropIndex("dbo.CursoDocente", new[] { "curso_cod" });
            DropIndex("dbo.Persona", new[] { "tipopersona_id" });
            DropIndex("dbo.CursoAlumno", new[] { "persona_id" });
            DropIndex("dbo.CursoAlumno", new[] { "curso_cod" });
            DropIndex("dbo.Curso", new[] { "ciclo_id" });
            DropIndex("dbo.Curso", new[] { "plan_id" });
            DropTable("dbo.TipoUsuario");
            DropTable("dbo.Usuario");
            DropTable("dbo.Notificacion");
            DropTable("dbo.HojaVidaDocenteFC");
            DropTable("dbo.HojaVidaDocenteFA");
            DropTable("dbo.HojaVidaDocenteEX");
            DropTable("dbo.HojaVidaDocenteCRP");
            DropTable("dbo.HojaVida");
            DropTable("dbo.TipoPersona");
            DropTable("dbo.TipoDocumento");
            DropTable("dbo.ConfigEntrega");
            DropTable("dbo.Unidad");
            DropTable("dbo.PlanEstudio");
            DropTable("dbo.Semestre");
            DropTable("dbo.MetadataDocumento");
            DropTable("dbo.Documento");
            DropTable("dbo.CursoDocente");
            DropTable("dbo.Persona");
            DropTable("dbo.CursoAlumno");
            DropTable("dbo.Curso");
            DropTable("dbo.Ciclo");
        }
    }
}
