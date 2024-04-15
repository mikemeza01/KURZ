using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    [Keyless]
    public class GetAdvices_Result
    {
        public int ID_ADVICE { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DATE_ADVICE { get; set; }

        [Display(Name = "Fecha")]
        public string DateAdviceFormat => DATE_ADVICE.ToString("dd/MM/yyyy");

        [Display(Name = "Fecha de creación")]
        public DateTime DATE_CREATE { get; set; }

        [Display(Name = "Fecha de creación")]
        public string DateCreateFormat => DATE_CREATE.ToString("dd/MM/yyyy");

        [Display(Name = "Fecha de Actualización")]
        public DateTime DATE_UPDATE { get; set; }
        public string DateUpdateFormat => DATE_UPDATE.ToString("dd/MM/yyyy");

        [Display(Name = "Enlace")]
        public string LINK { get; set; }

        [Display(Name = "Precio")]
        public double PRICE { get; set; }

        [Display(Name = "Tema")]
        public string TOPICNAME { get; set; }

        [Display(Name = "Descripción Tema")]
        public string TOPICDESCRIPTION { get; set; }

        [Display(Name = "Profesor")]
        public string TEACHERNAME { get; set; }
        public int IDTEACHER { get; set; }
        public int IDSTUDENT { get; set; }

        [Display(Name = "Estudiante")]
        public string STUDENTNAME { get; set; }

        [Display(Name = "Estado")]
        public string STATUSNAME { get; set; }

        [Display(Name = "Calificación")]
        public int GRADE { get; set; }

    }

    [Keyless]
    public class GetAdvicesById_Result : GetAdvices_Result
    {
    }

    [Keyless]
    public class GetAdvicesByTeacherId_Result : GetAdvices_Result
    {
    }
}
