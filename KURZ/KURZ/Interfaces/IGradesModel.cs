using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IGradesModel
    {
        public List<TeacherGradesView> TeacherGradesByUser(int teacherId);
          public object TeacherinfoByID(int adviceID);


    }
}
