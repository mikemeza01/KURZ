using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IStudentModel
    {
        //Consultar a Michael
        public List<Users>? StudentList();

        public string StudentCreate(Users student);
        public string StudentEdit(Users student);
    }
}
