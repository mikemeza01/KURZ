using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class StudentModel : IStudentModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public StudentModel(KurzContext context)
        {
            _context = context;
        }

       /* public List<Users>? UsersList()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
            }
        }*/

        public Users ValidateUser(Users student) {
            var email_login = student.EMAIL;
            var user_login = _context.Users.FirstOrDefault(e => e.EMAIL == email_login);
            if (user_login == null)
            {
                return null;
            }
            else {
                var password_decrypted = base64Decode(user_login.PASSWORD);

                if (password_decrypted == student.PASSWORD)
                {
                    return user_login;
                }
                else {
                    return null;
                }
            }
        }

        public int UserCreate(Users student)
        {
            try
            {
                //se encripta la clave puesta para el usuario
                if (student.PASSWORD != null)
                {
                    var passwordEncrypt = base64Encode(student.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    student.PASSWORD = passwordEncrypt;
                }
                student.CELLPHONE = "";
                student.PHOTO = "";
                student.PROFILE = "";
                student.ADDRESS = "";
                student.STATE = "";
                student.CITY = "";
                student.ID_ROL = 3; //id de rol estudiante
                student.CONFIRMATION = false;
                student.STATUS = true;
                student.PASSWORDTEMP = false;


                _context.Users.Add(student);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el estudiante:");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public string base64Encode(string sData) // Encode    
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
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
