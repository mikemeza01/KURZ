using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Users
    {

        [Key]
        [Display(Name = "ID de Usuario")]
        public int ID_USER { get; set; }

        [Required]
        [Display(Name = "Identificación")]
        public int IDENTIFICATION { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        [StringLength(20, ErrorMessage = "El nombre del usuario no puede exceder de 20 carácteres")]
        public string? USERNAME { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña no puede exceder de 30 carácteres")]
        public string? PASSWORD { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder de 20 carácteres")]
        public string? NAME { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = "Los apellidos no pueden exceder de 50 carácteres")]
        public string? LASTNAME { get; set; }

        [Display(Name = "Celular")]
        [StringLength(50, ErrorMessage = "El número de celular no pueden exceder de 15 carácteres")]
        public string? CELLPHONE { get; set; }

        [Display(Name = "Foto")]
        public string? PHOTO { get; set; }

        [Required]
        [Display(Name = "Correo Electrónico")]
        [StringLength(50, ErrorMessage = "El correo electrónico no pueden exceder de 50 carácteres")]
        public string? EMAIL { get; set; }

        [Display(Name = "Perfil")]
        [StringLength(2000, ErrorMessage = "El perfil no pueden exceder de 2000 carácteres")]
        public string? PROFILE { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200, ErrorMessage = "La dirección no pueden exceder de 200 carácteres")]
        public string? ADDRESS { get; set; }

        [Display(Name = "Estado / Provincia")]
        [StringLength(30, ErrorMessage = "El Estado / Provincia dirección no pueden exceder de 30 carácteres")]
        public string? STATE { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(30, ErrorMessage = "La ciudad no pueden exceder de 30 carácteres")]
        public string? CITY { get; set; }

        [Display(Name = "Rol")]
        public int ID_ROL { get; set; }

        [Display(Name = "País")]
        public int ID_COUNTRY { get; set; }

    }
}
