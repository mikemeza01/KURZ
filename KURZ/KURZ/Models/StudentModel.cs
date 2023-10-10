using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class StudentModel : IStudentModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;
        private readonly IUsersModel _usersModel;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public StudentModel(KurzContext context, IUsersModel userModel)
        {
            _context = context;
            _usersModel = userModel;
        }

       public List<Users>? StudentList()
        {
            try
            {
                return _context.Users.ToList(); //where id = 3
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de estudiantes: " + ex.Message);
            }
        }

        public string StudentCreate(Users student)
        {
            try
            {
                var user_exist = _usersModel.UserExist(student);

                if (user_exist != null)
                {
                    return user_exist;
                }

                //se encripta la clave puesta para el usuario
                if (student.PASSWORD != null)
                {
                    var passwordEncrypt = _usersModel.base64Encode(student.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    student.PASSWORD = passwordEncrypt;
                }
                student.USERNAME = student.IDENTICATION.ToString();
                student.CELLPHONE = "";
                student.PHOTO = "";
                student.PROFILE = "";
                student.ADDRESS = "";
                student.STATE = "";
                student.CITY = "";
                student.ID_COUNTRY = 52; //ID de País Costa Rica, esto para ponerlo para usuarios admin
                student.ID_ROL = 3; //id de rol estudiante
               

                _context.Users.Add(student);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el estudiante:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string StudentEdit(Users student_edit)
        {
            try
            {
                //validar si el correo cambio al editar el usuario
                if (_usersModel.UserEmail(student_edit.ID_USER) != student_edit.EMAIL)
                {
                    //valida si ya existe otro usuario con el mismo correo
                    var user_exist = _usersModel.UserExist(student_edit);

                    if (user_exist != null)
                    {
                        return user_exist;
                    }
                }

                if (student_edit.PASSWORD == null)
                {
                    var password = _usersModel.UserPassword(student_edit.ID_USER);
                    student_edit.PASSWORD = password;

                }
                else
                {
                    //se encripta la clave puesta para el usuario
                    if (student_edit.PASSWORD != null)
                    {
                        var passwordEncrypt = _usersModel.base64Encode(student_edit.PASSWORD);
                        //se pasa a la entidad la contraseña encriptada
                        student_edit.PASSWORD = passwordEncrypt;
                    }
                }

                student_edit.CELLPHONE = "";
                student_edit.PHOTO = "";
                student_edit.PROFILE = "";
                student_edit.ADDRESS = "";
                student_edit.STATE = "";
                student_edit.CITY = "";
                student_edit.ID_ROL = 3; //id de rol estudiante
                student_edit.ID_COUNTRY = 52; //ID de País Costa Rica, esto para ponerlo para usuarios admin

                _context.ChangeTracker.Clear();
                _context.Users.Update(student_edit);
                _context.SaveChanges();


                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar su usuario.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

    }
}
