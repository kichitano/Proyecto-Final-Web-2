namespace SistemaPortafolio.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CursoAlumno")]
    public partial class CursoAlumno
    {
        [Key]
        public int cursoalumno_id { get; set; }

        [Required]
        [StringLength(100)]
        public string curso_cod { get; set; }

        public int persona_id { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Persona Persona { get; set; }
    }
}
