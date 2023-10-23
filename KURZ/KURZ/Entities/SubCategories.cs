using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KURZ.Entities
{
    public class SubCategories
    {
        [Key]
        [Display(Name = "ID de la subcategoría")]
        public int ID_SUBCATEGORY { get; set; }

        [Display(Name = "Nombre de la Subcategoría")]
        [StringLength(50, ErrorMessage = "El nombre de la subcategoría no puede exceder de 50 carácteres")]
        public int NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder de 2 carácteres")]
        public int DESCRIPTION { get; set; }

        [Display(Name = "ID de Categoría")]
        public int ID_CATEGORY { get; set; }
    }
}
