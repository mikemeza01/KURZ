using KURZ.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace KURZ.Models
{
    public class ResetPasswordModel : IResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "La contraseña no puede exceder de 30 carácteres")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        [StringLength(30, ErrorMessage = "La contraseña no puede exceder de 30 carácteres")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
