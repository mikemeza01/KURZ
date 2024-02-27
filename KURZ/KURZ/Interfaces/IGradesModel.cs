using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IGradesModel
    {
        public List<TeacherGradesView> TeacherGradesByUser(int teacherId);
           public TeacherGradesView TeacherinfoByID(int adviceID);

    }
}
