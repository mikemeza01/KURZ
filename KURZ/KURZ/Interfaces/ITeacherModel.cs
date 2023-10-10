using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ITeacherModel
    {
        //Consultar a Michael
        public List<Users>? TeacherList();

        public string TeacherCreate(Users teacher);
    }
}
