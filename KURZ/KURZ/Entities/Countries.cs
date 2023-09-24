using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Countries
    {
        [Key]
        [Display(Name = "ID de País")]
        public int ID_COUNTRY { get; set; }
        [Display(Name = "Nombre de País")]
        [StringLength(80, ErrorMessage = "El nombre del país no puede exceder de 80 carácteres")]
        public string? NAME { get; set; }
        [Display(Name = "Nombre del ISO")]
        [StringLength(80, ErrorMessage = "El ISO no puede exceder de 80 carácteres")]
        public string? ISO_NAME { get; set; }
        [Display(Name = "ISO")]
        [StringLength(2, ErrorMessage = "El ISO no puede exceder de 2 carácteres")]
        public string? ISO { get; set; }
        [Display(Name = "ISO")]
        [StringLength(3, ErrorMessage = "El ISO3 no puede exceder de 3 carácteres")]
        public string? ISO3 { get; set; }
        [Display(Name = "Código de País")]
        public int NUMCODE { get; set; }
        [Display(Name = "Código de Teléfono de País")]
        public int PHONECODE { get; set; }
    }
}
