using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KURZ.Entities
{
    public class SubCategories
    {
        [Key]
        [Display(Name = "ID de la subcategoría")]
        public int ID_SUBCATEGORY { get; set; }

        [Required(ErrorMessage = "El nombre de la subcategoría es requerido")]
        [Display(Name = "Nombre de la Subcategoría")]
        [StringLength(50, ErrorMessage = "El nombre de la subcategoría debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 3)]
        public string? NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder de 1000 carácteres")]
        public string? DESCRIPTION { get; set; }

        [Required(ErrorMessage = "Seleccionar una categoría es requerido")]
        [Display(Name = "Categoría")]
        public int ID_CATEGORY { get; set; }
    }

    public class SubCategoriesView
    {
        public int ID_SUBCATEGORY { get; set; }

        [Display(Name = "Nombre de la Subcategoría")]
        public string? NAME { get; set; }

        [Display(Name = "Descripción")]
        public string? DESCRIPTION { get; set; }

        [Display(Name = "Categoría")]
        public string? CATEGORY { get; set; }
    }
}
