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
    using System.Data.Entity.Validation;
    using System.Web;
    using System.IO;
    using System.Collections;

    [Table("MetadataDocumento")]
    public partial class MetadataDocumento
    {
        [Key]
        public int metadata_id { get; set; }

        public int? documento_id { get; set; }

        [Required]
        [StringLength(100)]
        public string cod_curso { get; set; }

        public int persona_id { get; set; }

        public int semestre_id { get; set; }

        public int tipodocumento_id { get; set; }

        public int? id_unidad { get; set; }

        public int? pagina_total { get; set; }

        public int? palabra_total { get; set; }

        public int? caracter_total { get; set; }

        public int? linea_total { get; set; }

        public int? parrafo_total { get; set; }

        public int? celda { get; set; }

        public int? columna { get; set; }

        public int? diapositiva { get; set; }

        [StringLength(100)]
        public string tamanio { get; set; }

        [StringLength(100)]
        public string programa_nombre { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_creacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha_subida { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Unidad Unidad { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual Semestre Semestre { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }

        //Consulta 05

        public List<Semestre> listarsemestre()
        {
            var semestre = new List<Semestre>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    semestre = db.Semestre.ToList();
                }
            }
            catch (Exception e)
            {
                
            }
            return semestre;
        }

        public List<TipoDocumento> listartipodocumento()
        {
            var tipodoc = new List<TipoDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    tipodoc = db.TipoDocumento.Include("TipoPersona").Where(x => x.TipoPersona.nombre.Equals("Docente")).ToList();
                }
            }
            catch (Exception e)
            {
                
            }
            return tipodoc;
        }

        public MetadataDocumento listarmetadatareciente(string semestre,string tipodoc,string persona)
        {
            var meta = new MetadataDocumento();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    meta = db.MetadataDocumento.Include("Persona").Include("TipoDocumento").Include(x => x.Persona.TipoPersona).Where(x => x.Persona.codigo.Equals(persona.Trim()) || x.Persona.dni.Equals(persona.Trim())).Where(x => x.Persona.TipoPersona.nombre.Equals("Docente")).Where(x => x.semestre_id.ToString().Equals(semestre) && x.tipodocumento_id.ToString().Equals(tipodoc)).OrderByDescending(x => x.metadata_id).First();
                }
            }
            catch (Exception e)
            {
                
            }
            return meta;
        }

        public List<MetadataDocumento> listarmetadataantigua(string semestre, string tipodoc, string persona)
        {
            int sem = Convert.ToInt32(semestre);
            int tipd = Convert.ToInt32(tipodoc);
            int per = Convert.ToInt32(persona);

            List<MetadataDocumento> lista = new List<MetadataDocumento>();
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    Persona person = new Persona();

                    person = db.Persona.Where(x => x.dni.Equals(persona.Trim()) || x.codigo.Equals(persona.Trim())).SingleOrDefault();

                    var data = from MetadataDocumento in db.MetadataDocumento
                               where
                                 MetadataDocumento.persona_id == person.persona_id &&
                                 MetadataDocumento.tipodocumento_id == tipd &&
                                 MetadataDocumento.semestre_id == sem &&
                                 MetadataDocumento.metadata_id !=
                                   (from MetadataDocumento0 in db.MetadataDocumento
                                    where
       MetadataDocumento0.persona_id == person.persona_id &&
       MetadataDocumento0.tipodocumento_id == tipd &&
       MetadataDocumento0.semestre_id == sem
                                    select new
                                    {
                                        MetadataDocumento0.metadata_id
                                    }).Max(p => p.metadata_id)
                               orderby
                                 MetadataDocumento.metadata_id descending
                               select new
                               {
                                   metadata_id = MetadataDocumento.metadata_id,
                                   cod_curso = MetadataDocumento.cod_curso,
                                   persona_id = MetadataDocumento.persona_id,
                                   semestre_id = MetadataDocumento.semestre_id,
                                   tipodocumento_id = MetadataDocumento.tipodocumento_id,
                                   id_unidad = MetadataDocumento.id_unidad,
                                   pagina_total = MetadataDocumento.pagina_total,
                                   palabra_total = MetadataDocumento.palabra_total,
                                   caracter_total = MetadataDocumento.caracter_total,
                                   linea_total = MetadataDocumento.linea_total,
                                   parrafo_total = MetadataDocumento.parrafo_total,
                                   celda = MetadataDocumento.celda,
                                   columna = MetadataDocumento.columna,
                                   tamanio = MetadataDocumento.tamanio,
                                   programa_nombre = MetadataDocumento.programa_nombre,
                                   fecha_creacion = MetadataDocumento.fecha_creacion,
                                   fecha_subida = MetadataDocumento.fecha_subida
                               };

                    foreach(var c in data)
                    {
                        MetadataDocumento doc = new MetadataDocumento();

                        doc.metadata_id = c.metadata_id;
                        doc.cod_curso = c.cod_curso;
                        doc.persona_id = c.persona_id;
                        doc.semestre_id = c.semestre_id;
                        doc.tipodocumento_id = c.tipodocumento_id;
                        doc.id_unidad = c.id_unidad;
                        doc.pagina_total = c.pagina_total;
                        doc.palabra_total = c.palabra_total;
                        doc.caracter_total = c.caracter_total;
                        doc.linea_total = c.linea_total;
                        doc.parrafo_total = c.parrafo_total;
                        doc.celda = c.celda;
                        doc.columna = c.columna;
                        doc.tamanio = c.tamanio;
                        doc.programa_nombre = c.programa_nombre;
                        doc.fecha_creacion = c.fecha_creacion;
                        doc.fecha_subida = c.fecha_subida;

                        lista.Add(doc);
                    }
                }
            }
            catch (Exception e)
            {
                
            }
            return lista;
        }

        public string obtenertipodocumento(string tipo)
        {
            var semestre = new TipoDocumento();
            string extension = "";
            try
            {
                using (ModeloDatos db = new ModeloDatos())
                {
                    semestre = db.TipoDocumento.Where(x => x.tipodocumento_id.ToString().Equals(tipo)).SingleOrDefault();
                    extension = semestre.extension;
                }
            }
            catch (Exception e)
            {
                
            }
            return extension;
        }

        //fin consulta05
    }
}
