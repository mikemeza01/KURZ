using Microsoft.EntityFrameworkCore;

namespace KURZ.Entities
{
    [Keyless]
    public class GetAdvices_Result
    {
        public int ID_ADVICE { get; set; }
        public DateTime DATE_ADVICE { get; set; }
        public DateTime DATE_CREATE { get; set; }
        public DateTime DATE_UPDATE { get; set; }
        public string LINK { get; set; }
        public string TOPICNAME { get; set; }
        public string TOPICDESCRIPTION { get; set; }
        public string TEACHERNAME { get; set; }
        public string STUDENTNAME { get; set; }
        public string STATUSNAME { get; set; }

    }
}
