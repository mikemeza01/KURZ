using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Categories
    {
        [Key]
        [Display(Name = "ID de Categoría")]
        public int ID_CATEGORY { get; set; }

        [Display(Name = "Nombre de Categoría")]
        [StringLength(50, ErrorMessage = "El nombre de la categoría no puede exceder de 50 carácteres")]
        public int NAME { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder de 2 carácteres")]
        public int DESCRIPTION { get; set; }
    }
}
