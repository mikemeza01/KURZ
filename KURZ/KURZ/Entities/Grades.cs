using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KURZ.Entities
{
    public class Grades
    {
        [Key]
        [Display(Name = "ID de la Evaluación")]
        public int ID_GRADE { get; set; }

        [Display(Name = "Evaluación")]
        [Required(ErrorMessage = "La calificacion es requerida.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int? GRADE { get; set; }

        [Required(ErrorMessage = "El comentario es requerido.")]
        [Display(Name = "Comentario")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder de 500 carácteres")]
        public string? COMMENTARY { get; set; }

        [Display(Name = "Fecha de la Evaluación")]
        public DateTime DATE_GRADE { get; set; }

        [Display(Name = "ID de la Asesoría")]
        public int ID_ADVICE { get; set; }

        [Display(Name = "ID del Profesor")]
        public int ID_TEACHER { get; set; }

        [Display(Name = "ID del Estudiante")]
        public int ID_STUDENT { get; set; }

        
        [Display(Name = "Promedio Calificación")]
        public decimal? AverageRating { get; set; }
    }

    public class TeacherGradesView : Grades
    {
        public string? TeacherName { get; set; }

        public string? StudentName { get; set; }
        public string? Topic { get; set; }
        public string? Category { get; set; }
        public string? Subcategory { get; set; }
        public string? Status { get; set; }

    }//TeacherGrades

}
