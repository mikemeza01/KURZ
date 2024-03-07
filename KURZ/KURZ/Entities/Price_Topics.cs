using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Price_Topics
    {
        [Key]
        [Display(Name = "ID de Tema por profesor")]
        public int ID_PRICE_TOPIC { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Display(Name = "Precio")]
        public float PRICE { get; set; }

        [Required(ErrorMessage = "Debe existir un id de profesor")]
        [Display(Name = "ID Profesor")]
        public int ID_TEACHER { get; set; }

        [Required(ErrorMessage = "Debe existir un id de tema")]
        [Display(Name = "ID Tema")]
        public int ID_TOPIC { get; set; }
    }

    public class Price_Topics_Teacher
    {
        [Key]
        [Display(Name = "ID de Tema por profesor")]
        public int ID_PRICE_TOPIC { get; set; }

        [Display(Name = "Precio")]
        public float PRICE { get; set; }

        [Display(Name = "Nombre Profesor")]
        public string? TEACHER_NAME { get; set; }

        [Display(Name = "Tema")]
        public string? TOPIC_NAME { get; set; }
    }

}
