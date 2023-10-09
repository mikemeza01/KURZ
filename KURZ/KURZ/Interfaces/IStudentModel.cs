using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IStudentModel
    {
        //Consultar a Michael
        //public List<Users>? TeacherList();
        public Users ValidateUser(Users student);

        public int UserCreate(Users student);

        public string base64Encode(string sData);

        public string base64Decode(string sData);
    }
}
