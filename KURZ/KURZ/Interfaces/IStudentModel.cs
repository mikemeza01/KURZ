using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IStudentModel
    {
        //public List<Users>? StudentList();
        public Users ValidateUser(Users student);

        public string StudentCreate(Users student, string host);

        public string StudentEdit(Users student);
    }
}
