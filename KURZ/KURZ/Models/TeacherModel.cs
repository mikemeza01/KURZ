using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text;

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

        public Users ValidateUser(Users teacher) {
            var email_login = teacher.EMAIL;
            var user_login = _context.Users.FirstOrDefault(e => e.EMAIL == email_login);
            if (user_login == null)
            {
                return null;
            }
            else {
                var password_decrypted = base64Decode(user_login.PASSWORD);

                if (password_decrypted == teacher.PASSWORD)
                {
                    return user_login;
                }
                else {
                    return null;
                }
            }
        }

        public string UserCreate(Users teacher, string host)
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
                    var passwordEncrypt = base64Encode(teacher.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    teacher.PASSWORD = passwordEncrypt;
                }
                teacher.USERNAME = teacher.EMAIL;
                teacher.CELLPHONE = "";
                teacher.PHOTO = "";
                teacher.PROFILE = "";
                teacher.ADDRESS = "";
                teacher.STATE = "";
                teacher.CITY = "";
                teacher.ID_ROL = 2; //id de rol profesor
                teacher.CONFIRMATION = false;
                teacher.STATUS = true;
                teacher.PASSWORDTEMP = false;

                var token = _usersModel.CreateToken(10);

                teacher.TOKEN = token;
                _context.Users.Add(teacher);
                _context.SaveChanges();

                StringBuilder cuerpo = new StringBuilder("");
                cuerpo.Append(teacher.NAME + teacher.LASTNAME);
                cuerpo.Append("<br>");
                cuerpo.Append("<br>");
                cuerpo.Append("Se ha creado tu cuenta en KURZ. Debes confirmar la cuenta para poder empezar a utilizar la plataforma");
                cuerpo.Append("<br>");
                cuerpo.Append("<a href='" + host + "/Authentication/ConfirmationAccount/?username=" + teacher.EMAIL + "&token=" + token + "'> Confirmar cuenta</a>");
                cuerpo.Append("<br>");
                cuerpo.Append("<br>");
                cuerpo.Append("Saludos cordiales,");
                cuerpo.Append("<br>");
                cuerpo.Append("KURZ");

                try
                {
                    _usersModel.SendEmail(teacher.EMAIL, "Confirmar Cuenta", cuerpo.ToString());
                }
                catch (Exception ex)
                {
                    return "Usuario creado pero hubo un error al enviar el correo de confirmación de cuenta, pongase en contacto con el administrador.";
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el profesor:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }
        public string TeacherEdit(Users teacher)
        {
            try
            {
                //validar si el correo cambio al editar el usuario
                if (_usersModel.UserEmail(teacher.ID_USER) != teacher.EMAIL)
                {
                    //valida si ya existe otro usuario con el mismo correo
                    var user_exist = _usersModel.UserExist(teacher);

                    if (user_exist != null)
                    {
                        return user_exist;
                    }
                }

                if (teacher.PASSWORD == null)
                {
                    var password = _usersModel.UserPassword(teacher.ID_USER);
                    teacher.PASSWORD = password;

                }

                teacher.CELLPHONE = "";
                teacher.PHOTO = "";

                teacher.ADDRESS = "";
                teacher.STATE = "";
                teacher.CITY = "";
                teacher.ID_ROL = 2; //id de rol profesor

                _context.ChangeTracker.Clear();
                _context.Users.Update(teacher);
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
    }
}
