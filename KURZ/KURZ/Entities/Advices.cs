using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KURZ.Entities
{
    public class Advices
    {
        [Key]
        public int ID_ADVICE { get; set; }

        [Display(Name = "Fecha del advice")]
        public DateTime DATE_ADVICE { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime DATE_CREATE { get; set; }

        [Display(Name = "Fecha de actualización")]
        public DateTime DATE_UPDATE { get; set; }

        [Display(Name = "Link")]
        [StringLength(500, ErrorMessage = "El link no puede exceder de 500 carácteres")]
        public string LINK { get; set; }

        [Display(Name = "Precio")]
        public decimal PRICE { get; set; }

        [Display(Name = "Id del Tema")]
        public int ID_TOPIC { get; set; }

        [Display(Name = "Id del profesor")]
        public int ID_TEACHER { get; set; }

        [Display(Name = "Id del estudiante")]
        public int ID_STUDENT { get; set; }

        [Display(Name = "Id del status")]
        public int ID_STATUS { get; set; }
    }
}
