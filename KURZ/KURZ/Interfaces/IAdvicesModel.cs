using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IAdvicesModel
    {
        public List<GetAdvices_Result> GetAdvices();
        public GetAdvicesById_Result GetAdvicesById(int id);
        public List<GetAdvicesByTeacherId_Result> GetAdvicesByTeacherId(int id);
        public List<GetAdvicesByTeacherId_Result> GetAdvicesByStudentId(int id);

        public TeacherTopicsView TopicTeacherDetails(int ID_TEACHER, int ID_TOPIC);
    }
}
