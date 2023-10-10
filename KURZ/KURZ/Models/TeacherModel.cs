using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class TeacherModel : ITeacherModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;
        private readonly IUsersModel _usersModel;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public TeacherModel(KurzContext context, IUsersModel usersModel)
        {
            _context = context;
            _usersModel = usersModel;
        }

        public List<Users>? TeacherList()
         {
             try
             {
                 return _context.Users.ToList(); //where id_rol == 2
             }
             catch (Exception ex)
             {
                 throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
             }
         }

        public string TeacherCreate(Users teacher)
        {
            try
            {
                var user_exist = _usersModel.UserExist(teacher);

                if (user_exist != null)
                {
                    return user_exist;
                }

                //se encripta la clave puesta para el usuario
                if (teacher.PASSWORD != null)
                {
                    var passwordEncrypt = _usersModel.base64Encode(teacher.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    teacher.PASSWORD = passwordEncrypt;
                }
                teacher.USERNAME = teacher.IDENTICATION.ToString();
                teacher.CELLPHONE = "";
                teacher.PHOTO = "";
                teacher.PROFILE = "";
                teacher.ADDRESS = "";
                teacher.STATE = "";
                teacher.CITY = "";
                teacher.ID_COUNTRY = 52; //ID de País Costa Rica
                teacher.ID_ROL = 2; //id de rol administrador
               

                _context.Users.Add(teacher);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el profesor:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }
    }
}
