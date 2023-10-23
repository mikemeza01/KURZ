using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Entities
{
    public class Countries
    {
        [Key]
        [Display(Name = "ID de País")]
        [StringLength(80, ErrorMessage = "El ID del país no puede exceder de 80 carácteres")]
        public int ID_COUNTRY { get; set; }
        [Display(Name = "Nombre de País")]
        [StringLength(80, ErrorMessage = "El nombre del país no puede exceder de 80 carácteres")]
        public string? NAME { get; set; }
        [Display(Name = "Nombre del ISO")]
        [StringLength(80, ErrorMessage = "El ISO no puede exceder de 80 carácteres")]
        public string? ISO_NAME { get; set; }
        [Display(Name = "ISO")]
        [StringLength(80, ErrorMessage = "El ISO no puede exceder de 2 carácteres")]
        public string? ISO { get; set; }
        [Display(Name = "ISO")]
        [StringLength(80, ErrorMessage = "El ISO3 no puede exceder de 3 carácteres")]
        public string? ISO3 { get; set; }
        [Display(Name = "Código de País")]
        [StringLength(80, ErrorMessage = "El código del país no puede exceder de 80 carácteres")]

        public int NUMCODE { get; set; }


    }
}
