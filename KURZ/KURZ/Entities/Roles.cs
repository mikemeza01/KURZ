using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Roles
    {
        [Key]
        [Display(Name = "ID Rol")]
        public int ID_ROL { get; set; }
        [Display(Name = "Nombre de Rol")]
        [StringLength(20, ErrorMessage = "El nombre del rol no puede exceder de 20 carácteres")]
        public string? NAME { get; set; }
        [Display(Name = "Descripción de Rol")]
        [StringLength(100, ErrorMessage = "La descripción del rol no puede exceder de 100 carácteres")]
        public string? DESCRIPTION { get; set; }

    }
}
