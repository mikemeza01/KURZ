using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IStudentModel
    {
        //Consultar a Michael
        public List<Users>? StudentList();

        public string StudentCreate(Users student, string host);
        public string StudentEdit(Users student, IFormFile photoFile);
    }
}
