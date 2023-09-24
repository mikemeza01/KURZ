using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Status
    {
        [Key]
        [Display(Name = "ID de Estado")]
        public int ID_STATE { get; set; }
        [Display(Name = "Nombre de Estado")]
        [StringLength(20, ErrorMessage = "El nombre del Estado no puede exceder de 20 carácteres")]
        public string? NAME { get; set; }
        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder de 500 carácteres")]
        public string? DESCRIPTION { get; set; }

    }
}
