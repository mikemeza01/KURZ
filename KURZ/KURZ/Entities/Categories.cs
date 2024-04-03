using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Categories
    {
        [Key]
        [Display(Name = "ID de Categoría")]
        public int ID_CATEGORY { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [Display(Name = "Nombre de Categoría")]
        [StringLength(50, ErrorMessage = "El nombre de la categoría debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 3)]
        public string? NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder de 1000 carácteres")]
        public string? DESCRIPTION { get; set; }
        
    }
}
