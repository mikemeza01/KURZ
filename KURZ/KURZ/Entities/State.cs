using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    //State se refiera a los estados o provincias en los datos del usuario
    public class State
    {
        [Key]
        [Display(Name = "ID de Estado / Provincia")]
        public int ID_STATE { get; set; }
        [Display(Name = "Nombre de Estado / Provincia")]
        [StringLength(30, ErrorMessage = "El nombre del Estado / Provincia no puede exceder de 30 carácteres")]
        public string? NAME { get; set; }
        [Display(Name = "ISO")]
        [StringLength(2, ErrorMessage = "El nombre del ISO no puede exceder de 2 carácteres")]
        public string? ISO { get; set; }
        [Display(Name = "País")]
        public int ID_COUNTRY { get; set; }

    }
}
