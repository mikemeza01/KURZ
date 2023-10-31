using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KURZ.Entities
{
    public class Users
    {

        [Key]
        [Display(Name = "ID de Usuario")]
        public int ID_USER { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [Display(Name = "Identificación")]
        public int? IDENTICATION { get; set; }

        [Display(Name = "Nombre de Usuario")]
        public string? USERNAME { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña no puede exceder de 30 carácteres")]
        public string? PASSWORD { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder de 20 carácteres")]
        public string? NAME { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = "Los apellidos no pueden exceder de 50 carácteres")]
        public string? LASTNAME { get; set; }

        [Display(Name = "Celular")]
        [StringLength(100, ErrorMessage = "El número de celular no pueden exceder de 15 carácteres")]
        public string? CELLPHONE { get; set; }

        [Display(Name = "Foto")]
        public string? PHOTO { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [Display(Name = "Correo Electrónico")]
        [StringLength(50, ErrorMessage = "El correo electrónico no pueden exceder de 50 carácteres")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
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

        [Display(Name = "Confirmación")]
        public bool CONFIRMATION { get; set; } //1 confirmo la cuenta 0 no la a confirmado

        [Display(Name = "Estado")]
        public bool STATUS { get; set; } //1 activo 0 inactivo

        [Display(Name = "Contraseña Temporal")]
        public bool PASSWORDTEMP { get; set; } //1 la contraseña es temporal 0 la contraseña no es temporal

        public string? TOKEN { get; set; } //token de seguridad

    }

    public class UsersBinding
    {
        [Key]
        [Display(Name = "ID de Usuario")]
        public int ID_USER { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]

        [Display(Name = "Identificación")]
        [Range(1000, 999999999999999999, ErrorMessage = "La identificación debe tener mínimo 4 y máximo 18 carácteres ")]
        public int? IDENTICATION { get; set; }

        [Display(Name = "Nombre de Usuario")]
        public string? USERNAME { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "La contraseña debe ser mínimo de 8 carácteres y contenener al menos una mayúsculas, una minúscula, un número y un caracter especial (por ejemplo, !@#$%^&*)\"")]
        public string? PASSWORD { get; set; }

        [Required(ErrorMessage = "Repetir contraseña es requerida")]
        [Display(Name = "Repetir Contraseña")]
        [StringLength(30, ErrorMessage = "Repetir la contraseña debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 8)]
        [Compare("PASSWORD", ErrorMessage = "La contraseña y repetir contraseña deben coincidir")]
        public string? PASSWORD_REPEAT { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(20, ErrorMessage = "El nombre debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 2)]
        public string? NAME { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = "Los apellidos deben tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 2)]

        public string? LASTNAME { get; set; }

        [Display(Name = "Celular")]
        [StringLength(15, ErrorMessage = "El número de celular solo puede tener un máximo de 15 carácteres ")]
        public string? CELLPHONE { get; set; }

        [Display(Name = "Foto")]
        public string? PHOTO { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [Display(Name = "Correo Electrónico")]
        [StringLength(100, ErrorMessage = "El correo electrónico debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 4)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? EMAIL { get; set; }

        [Display(Name = "Perfil")]
        [StringLength(2000, ErrorMessage = "El perfil solo puede tener un máximo de 2000 carácteres ")]
        public string? PROFILE { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200, ErrorMessage = "El perfil solo puede tener un máximo de 200 carácteres ")]
        public string? ADDRESS { get; set; }

        [Display(Name = "Estado / Provincia")]
        [StringLength(30, ErrorMessage = "El Estado / Provincia solo puede tener un máximo de 2000 carácteres ")]
        public string? STATE { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(30, ErrorMessage = "El ciudad solo puede tener un máximo de 30 carácteres ")]
        public string? CITY { get; set; }

        [Display(Name = "Rol")]
        public int ID_ROL { get; set; }

        [Required(ErrorMessage = "El país es requerido")]
        [Display(Name = "País")]
        public int ID_COUNTRY { get; set; }

        [Display(Name = "Confirmación")]
        public bool CONFIRMATION { get; set; } //1 confirmo la cuenta 0 no la a confirmado

        [Display(Name = "Estado")]
        public bool STATUS { get; set; } //1 activo 0 inactivo

        [Display(Name = "Contraseña Temporal")]
        public bool PASSWORDTEMP { get; set; } //1 la contraseña es temporal 0 la contraseña no es temporal

        public string? TOKEN { get; set; } //token de seguridad
    }

    public class UsersBindingEdit
    {
        [Key]
        [Display(Name = "ID de Usuario")]
        public int ID_USER { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]

        [Display(Name = "Identificación")]
        [Range(1000, 999999999999999999, ErrorMessage = "La identificación debe tener mínimo 4 y máximo 18 carácteres ")]
        public int? IDENTICATION { get; set; }

        [Display(Name = "Nombre de Usuario")]
        public string? USERNAME { get; set; }

        [Display(Name = "Contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 8)]
        [RegularExpression("^((?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "La contraseña debe ser mínimo de 8 carácteres y contenener al menos una mayúsculas, una minúscula, un número y un caracter especial (por ejemplo, !@#$%^&*)\"")]
        public string? PASSWORD { get; set; }

        [Display(Name = "Repetir Contraseña")]
        [StringLength(30, ErrorMessage = "Repetir la contraseña debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 8)]
        [Compare("PASSWORD", ErrorMessage = "La contraseña y repetir contraseña deben coincidir")]
        public string? PASSWORD_REPEAT { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(20, ErrorMessage = "El nombre debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 2)]
        public string? NAME { get; set; }

        [Required(ErrorMessage = "Los apellidos son requeridos")]
        [Display(Name = "Apellidos")]
        [StringLength(50, ErrorMessage = "Los apellidos deben tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 2)]

        public string? LASTNAME { get; set; }

        [Display(Name = "Celular")]
        [StringLength(15, ErrorMessage = "El número de celular solo puede tener un máximo de 15 carácteres ")]
        public string? CELLPHONE { get; set; }

        [Display(Name = "Foto")]
        public string? PHOTO { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [Display(Name = "Correo Electrónico")]
        [StringLength(100, ErrorMessage = "El correo electrónico debe tener mínimo {2} y máximo {1} carácteres ", MinimumLength = 4)]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? EMAIL { get; set; }

        [Display(Name = "Perfil")]
        [StringLength(2000, ErrorMessage = "El perfil solo puede tener un máximo de 2000 carácteres ")]
        public string? PROFILE { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200, ErrorMessage = "El perfil solo puede tener un máximo de 200 carácteres ")]
        public string? ADDRESS { get; set; }

        [Display(Name = "Estado / Provincia")]
        [StringLength(30, ErrorMessage = "El Estado / Provincia solo puede tener un máximo de 2000 carácteres ")]
        public string? STATE { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(30, ErrorMessage = "El ciudad solo puede tener un máximo de 30 carácteres ")]
        public string? CITY { get; set; }

        [Display(Name = "Rol")]
        public int ID_ROL { get; set; }

        [Required(ErrorMessage = "El país es requerido")]
        [Display(Name = "País")]
        public int ID_COUNTRY { get; set; }

        [Display(Name = "Confirmación")]
        public bool CONFIRMATION { get; set; } //1 confirmo la cuenta 0 no la a confirmado

        [Display(Name = "Estado")]
        public bool STATUS { get; set; } //1 activo 0 inactivo

        [Display(Name = "Contraseña Temporal")]
        public bool PASSWORDTEMP { get; set; } //1 la contraseña es temporal 0 la contraseña no es temporal

        public string? TOKEN { get; set; } //token de seguridad
    }

    public class Login
    {

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? EMAIL { get; set; }

        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [Display(Name = "Contraseña")]
        public string? PASSWORD { get; set; }

    
    }

    public class UserDetails : Users
    {
        public byte[]? ProfilePicture { get; set; }
    }
}
