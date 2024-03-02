using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KURZ.Entities
{
    public class Topics
    {
        [Key]
        [Display(Name = "Id del Tema")]
        public int ID_TOPIC { get; set; }

        [Required(ErrorMessage = "El nombre del tema es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del tema debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 3)]
        [Display(Name = "Nombre del Tema")]
        public string? NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder de 1000 carácteres")]
        public string? DESCRIPTION { get; set; }

        [Required(ErrorMessage = "Seleccionar una categoría es requerido")]
        [Display(Name = "Categoría")]
        public int ID_CATEGORY { get; set; }

        [Required(ErrorMessage = "Seleccionar una subcategoría es requerido")]
        [Display(Name = "Subcategoría")]
        public int ID_SUBCATEGORY { get; set; }

        [Required(ErrorMessage = "Seleccionar un estado es requerido")]
        [Display(Name = "Estado")]
        public bool STATUS { get; set; }
    }

    public class TopicsView
    {
        public int ID_TOPIC { get; set; }

        [Display(Name = "Nombre del Tema")]
        public string? NAME { get; set; }

        [Display(Name = "Descripción")]
        public string? DESCRIPTION { get; set; }

        [Display(Name = "Categoría")]
        public string? CATEGORY { get; set; }

        [Display(Name = "Subcategoría")]
        public string? SUBCATEGORY { get; set; }


        [Display(Name = "Estado")]
        public bool STATUS { get; set; }
    }
}
