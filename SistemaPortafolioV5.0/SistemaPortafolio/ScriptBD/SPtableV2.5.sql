CREATE DATABASE db_portafolio
go
USE db_portafolio
go

if (not exists(select 1 from sys.tables where name = 'Semestre'))
    CREATE TABLE dbo.Semestre (
       semestre_id    int identity(1,1) NOT NULL,
       nombre        varchar(100) NOT NULL unique,
       anio 	     char(4) NOT NULL,
       fechainicio   smalldatetime NOT NULL,
       fechafin      smalldatetime NOT NULL,
	   estado 	varchar(100) not null,
       PRIMARY KEY (semestre_id)
      )
go


if (not exists(select 1 from sys.tables where name = 'PlanEstudio'))
    CREATE TABLE dbo.PlanEstudio (
       plan_id         int identity(1,1) NOT NULL,
       semestre_id     int,
       nombre          varchar(100) NULL unique,
       estado          varchar(10) NOT NULL,
       PRIMARY KEY (plan_id),
       FOREIGN KEY (semestre_id) REFERENCES SEMESTRE
)
go

if (not exists(select 1 from sys.tables where name = 'Ciclo'))
    CREATE TABLE dbo.Ciclo (
       ciclo_id      int identity(1,1) NOT NULL,
       nombre        varchar(100) NULL unique,
       descripcion   text,
	   estado 	varchar(100) not null,
       PRIMARY KEY (ciclo_id)
)
go

if (not exists(select 1 from sys.tables where name = 'Curso'))
    CREATE TABLE dbo.Curso (
       curso_cod        varchar(100) NOT NULL unique,
	   plan_id int not null,
       ciclo_id         int NOT NULL,
       nombre          varchar(150) NOT NULL unique,
       credito        int NOT NULL,
	   horasteoria     int NOT NULL,
       horaspractica   int NOT NULL,
       totalhoras      int NOT NULL,
       prerequisito    varchar(150) NULL,
	   estado 	varchar(100) not null,
       PRIMARY KEY (curso_cod),
       FOREIGN KEY (ciclo_id) REFERENCES Ciclo,
	   FOREIGN KEY (plan_id) REFERENCES PlanEstudio
)
go

if (not exists(select 1 from sys.tables where name = 'TipoPersona'))
    CREATE TABLE dbo.TipoPersona (
       tipopersona_id       int identity(1,1) NOT NULL,
       nombre               varchar(100) NOT NULL UNIQUE,
	   estado 	varchar(100) not null,
       PRIMARY KEY (tipopersona_id)
)
go

if (not exists(select 1 from sys.tables where name = 'Persona'))
    CREATE TABLE dbo.Persona (
       persona_id      int identity(1,1) NOT NULL,
       tipopersona_id  int NOT NULL,
       dni            varchar(8) NOT NULL UNIQUE,
       codigo         varchar(10) NULL UNIQUE,
       nombre         varchar(100) NOT NULL,
       apellido       varchar(100) NOT NULL,
       email          varchar(100) NULL UNIQUE,
       celular	      varchar(15) NULL,
	   estado 	varchar(100) not null,
       PRIMARY KEY (persona_id), 
       FOREIGN KEY (tipopersona_id) REFERENCES TipoPersona
)
go

if (not exists(select 1 from sys.tables where name = 'CursoDocente'))
    CREATE TABLE dbo.CursoDocente (
	   cursodocente_id	int identity(1,1) NOT NULL,
       curso_cod        varchar(100) NOT NULL,
       persona_id          int NOT NULL,
       PRIMARY KEY (cursodocente_id),
       FOREIGN KEY (curso_cod) REFERENCES Curso,
       FOREIGN KEY (persona_id) REFERENCES Persona
)
go

if (not exists(select 1 from sys.tables where name = 'CursoAlumno')) --agregado
    CREATE TABLE dbo.CursoAlumno (
	   cursoalumno_id	int identity(1,1) NOT NULL,
       curso_cod        varchar(100) NOT NULL,
       persona_id          int NOT NULL,
       PRIMARY KEY (cursoalumno_id),
       FOREIGN KEY (curso_cod) REFERENCES Curso,
       FOREIGN KEY (persona_id) REFERENCES Persona
)
go

if (not exists(select 1 from sys.tables where name = 'TipoUsuario'))
    CREATE TABLE dbo.TipoUsuario (
       tipousuario_id       int identity(1,1) NOT NULL,
       nombre               varchar(100) NOT NULL UNIQUE, 
       descripcion          text NULL,
	   estado 	varchar(100) not null,
       PRIMARY KEY (tipousuario_id)
)
go

if (not exists(select 1 from sys.tables where name = 'Usuario'))
    CREATE TABLE dbo.Usuario (
       usuario_id      int identity(1,1) NOT NULL,
       persona_id      int NOT NULL,  
       tipousuario     int NOT NULL,
       nombre          varchar(100) NOT NULL UNIQUE,
       clave           varchar(100) NOT NULL,
       avatar          varchar(200) NOT NULL,
       estado          varchar(10) NOT NULL,       
       PRIMARY KEY (usuario_id),
       FOREIGN KEY (persona_id)  REFERENCES Persona,
	   FOREIGN KEY (tipousuario)  REFERENCES TipoUsuario   
)
go

if (not exists(select 1 from sys.tables where name = 'HojaVida'))
    CREATE TABLE dbo.HojaVida (
	   hojavida_id	int identity(1,1) NOT NULL,
       persona_id        int NOT NULL,
       PRIMARY KEY (hojavida_id),
       FOREIGN KEY (persona_id) REFERENCES Persona
)
go

if (not exists(select 1 from sys.tables where name = 'HojaVidaDocenteFA'))
    CREATE TABLE dbo.HojaVidaDocenteFA (
       hojavidadocentefa_id        int identity(1,1) NOT NULL,
       hojavida_id         int NOT NULL,
       institucion             varchar(150) NOT NULL,
       titulo        varchar(150) NOT NULL,
       PRIMARY KEY (hojavidadocentefa_id),
       FOREIGN KEY (hojavida_id) REFERENCES HojaVida
)
go

if (not exists(select 1 from sys.tables where name = 'HojaVidaDocenteFC'))
    CREATE TABLE dbo.HojaVidaDocenteFC (
       hojavidadocentefc_id        int identity(1,1) NOT NULL,
       hojavida_id         int NOT NULL,
       institucion             varchar(150) NOT NULL,
	   tiempo_duracion   varchar(150) NOT NULL,
       duracion   varchar(150) NOT NULL,
	   curso   varchar(150) NOT NULL,
       PRIMARY KEY (hojavidadocentefc_id),
       FOREIGN KEY (hojavida_id) REFERENCES HojaVida
)
go

if (not exists(select 1 from sys.tables where name = 'HojaVidaDocenteCRP'))
    CREATE TABLE dbo.HojaVidaDocenteCRP (
       hojavidadocentecrp_id        int identity(1,1) NOT NULL,
       hojavida_id         int NOT NULL,
       certificacion             varchar(150) NOT NULL,
	   institucion   varchar(150) NOT NULL,
       año   varchar(20) NOT NULL,
       PRIMARY KEY (hojavidadocentecrp_id),
       FOREIGN KEY (hojavida_id) REFERENCES HojaVida
)
go

if (not exists(select 1 from sys.tables where name = 'HojaVidaDocenteEX'))
    CREATE TABLE dbo.HojaVidaDocenteEX (
       hojavidadocenteex_id        int identity(1,1) NOT NULL,
       hojavida_id         int NOT NULL,
       institucion             varchar(150) NOT NULL,
       fechainicio   smalldatetime NOT NULL,
	   fechafin   smalldatetime NOT NULL,
	   funcion	text,
       PRIMARY KEY (hojavidadocenteex_id),
       FOREIGN KEY (hojavida_id) REFERENCES HojaVida
)
go

if (not exists(select 1 from sys.tables where name = 'TipoDocumento'))
    CREATE TABLE dbo.TipoDocumento (
	   tipodocumento_id        int identity(1,1) NOT NULL,
	   tipopersona_id	int not null,
	   extension	varchar(100) not null,
       nombre             varchar(150) NOT NULL
       PRIMARY KEY (tipodocumento_id),
	   FOREIGN KEY (tipopersona_id) REFERENCES TipoPersona --agregado
)
go

if (not exists(select 1 from sys.tables where name = 'Unidad')) -- NUEVA TABLA CREADA
    CREATE TABLE dbo.Unidad (
	id_unidad 	int identity(1,1) NOT NULL,
	id_semestre int not null,
	descripcion 				varchar(150),
	estado	varchar(50) not null,
	PRIMARY KEY (id_unidad),
	FOREIGN KEY (id_semestre) REFERENCES Semestre
)
go

if (not exists(select 1 from sys.tables where name = 'Documento'))
    CREATE TABLE dbo.Documento (
	   documento_id        int identity(1,1) NOT NULL,
       tipodocumento_id         int NOT NULL,
	   persona_id        int NOT NULL,
	   id_unidad        int, --agregado!!
	   curso_cod 	varchar(100) not null,
       archivo             varchar(150) NOT NULL,
	   descripcion             text NOT NULL,
       fecha_entrega   smalldatetime NOT NULL,
	   hora_entrega   smalldatetime NOT NULL,
	   extension       char(10) NULL,
       tamanio         varchar(50) NULL,
	   estado 		varchar(50) not null,
       PRIMARY KEY (documento_id),
	   FOREIGN KEY (tipodocumento_id) REFERENCES TipoDocumento,
       FOREIGN KEY (persona_id) REFERENCES Persona,
	   FOREIGN KEY (curso_cod) REFERENCES Curso,
	   FOREIGN KEY (id_unidad) REFERENCES Unidad
)
go


if (not exists(select 1 from sys.tables where name = 'MetadataDocumento'))
    CREATE TABLE dbo.MetadataDocumento (
	metadata_id int identity(1,1) not null, --codigo del curso
	documento_id	int,
	cod_curso	varchar(100) not null, --codigo del curso
	persona_id	int not null, --persona que subio el documento
	semestre_id int not null, --semestre que se subio el documento
	tipodocumento_id int not null, --semestre que se subio el documento
	id_unidad int, --unidad de subida del documento
	pagina_total int, --word, no aparece en excel
	palabra_total int, --word, no aparece en excel
	caracter_total int, --word, no aparece en excel
	linea_total int, --word, no aparece en excel
	parrafo_total int, --word, no aparece en excel
	celda int,
	columna int,
	diapositiva int,
	tamanio varchar(100), --word y excel
	programa_nombre varchar(100), --word y excel
	fecha_creacion smalldatetime, --word y excel
	fecha_subida smalldatetime,
	PRIMARY KEY (metadata_id),
	FOREIGN KEY (documento_id) REFERENCES Documento,
	FOREIGN KEY (cod_curso) REFERENCES Curso,
    FOREIGN KEY (persona_id) REFERENCES Persona,
	FOREIGN KEY (semestre_id) REFERENCES Semestre,
	FOREIGN KEY (tipodocumento_id) REFERENCES TipoDocumento,
	FOREIGN KEY (id_unidad) REFERENCES Unidad
)
go

if (not exists(select 1 from sys.tables where name = 'ConfigEntrega'))
    CREATE TABLE dbo.ConfigEntrega (
	configentrega_id 	int identity(1,1) NOT NULL,
	id_unidad 			int not null,
	nombre 				varchar(150),
	fecha_inicio 		smalldatetime NOT NULL,
	fecha_fin 			smalldatetime NOT NULL,
	estado			varchar(50) not null,
	PRIMARY KEY (configentrega_id),
	FOREIGN KEY (id_unidad) REFERENCES Unidad
)
go

if (not exists(select 1 from sys.tables where name = 'Notificacion'))
    CREATE TABLE dbo.Notificacion (
	   notificacion_id        int identity(1,1) NOT NULL,
       persona_emisor         int NOT NULL,
	   persona_receptor	      int NOT NULL,
	   titulo             varchar(150) NOT NULL,
	   asunto             varchar(150) NOT NULL,
       mensaje             varchar(150) NOT NULL,
	   fecha_emision   smalldatetime NOT NULL,
	   estado varchar(100) not null,
       PRIMARY KEY (notificacion_id),
	   FOREIGN KEY (persona_emisor) REFERENCES Persona,
	   FOREIGN KEY (persona_receptor) REFERENCES Persona
)
go