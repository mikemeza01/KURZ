using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IAdvicesModel
    {

        public string AdviceCreate(Advices advice, string host);

        public string AdvicesEdit(Advices advice);

        public string notifyStudent(Advices advice, bool confirm, string host);

        public Advices AdviceDetail(int? ID);

        public List<GetAdvices_Result> GetAdvices();
        public GetAdvicesById_Result GetAdvicesById(int id);
        public List<GetAdvicesByTeacherId_Result> GetAdvicesByTeacherId(int id);
        public List<GetAdvicesByTeacherId_Result> GetAdvicesByStudentId(int id);

        public TeacherTopicsView TopicTeacherDetails(int ID_TEACHER, int ID_TOPIC);

        public List<Advices> AdvicesforTeacherAndDate(int ID_TEACHER, DateTime DATE_SELECTED);
    }
}
