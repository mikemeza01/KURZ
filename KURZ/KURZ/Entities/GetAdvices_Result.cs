using Microsoft.EntityFrameworkCore;

namespace KURZ.Entities
{
    [Keyless]
    public class GetAdvices_Result
    {
        public int ID_ADVICE { get; set; }
        public DateTime DATE_ADVICE { get; set; }
        public string DateAdviceFormat => DATE_ADVICE.ToString("dd/MM/yyyy");
        public DateTime DATE_CREATE { get; set; }
        public string DateCreateFormat => DATE_CREATE.ToString("dd/MM/yyyy");
        public DateTime DATE_UPDATE { get; set; }
        public string DateUpdateFormat => DATE_UPDATE.ToString("dd/MM/yyyy");
        public string LINK { get; set; }
        public double PRICE { get; set; }
        public string TOPICNAME { get; set; }
        public string TOPICDESCRIPTION { get; set; }
        public string TEACHERNAME { get; set; }
        public int IDTEACHER { get; set; }
        public int IDSTUDENT { get; set; }
        public string STUDENTNAME { get; set; }
        public string STATUSNAME { get; set; }
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
