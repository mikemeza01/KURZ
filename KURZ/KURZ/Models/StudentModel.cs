using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.DependencyResolver;
using System.Text;



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

        //public List<Users>? StudentList()
        //{
        //    try
        //    {
        //        return _context.Users.ToList(); //where id = 3
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error al obtener la lista de estudiantes: " + ex.Message);
        //    }
        //}

        public Users ValidateUser(Users student)
        {
            var email_login = student.EMAIL;
            var user_login = _context.Users.FirstOrDefault(e => e.EMAIL == email_login);
            if (user_login == null)
            {
                return null;
            }
            else
            {
                var password_decrypted = _usersModel.base64Decode(user_login.PASSWORD);

                if (password_decrypted == student.PASSWORD)
                {
                    return user_login;
                }
                else
                {
                    return null;
                }
            }
        }

        public string StudentCreate(Users student, string host)
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
                student.USERNAME = student.EMAIL;
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

                var token = _usersModel.CreateToken(10);

                student.TOKEN = token;

                _context.Users.Add(student);
                _context.SaveChanges();

                //StringBuilder cuerpo = new StringBuilder("");
                string cuerpo = _usersModel.ArmarHTMLCC(student, host);
                //cuerpo.Append(student.NAME + student.LASTNAME);
                //cuerpo.Append("<br>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("Se ha creado tu cuenta en KURZ. Debes confirmar la cuenta para poder empezar a utilizar la plataforma");
                //cuerpo.Append("<br>");
                //cuerpo.Append("<a href='" + host + "/Authentication/ConfirmationAccount/?username=" + student.EMAIL + "&token=" + token + "'> Confirmar cuenta</a>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("Saludos cordiales,");
                //cuerpo.Append("<br>");
                //cuerpo.Append("KURZ");

                try
                {
                   //_usersModel.SendEmail(student.EMAIL, "Confirmar Cuenta", cuerpo.ToString());
                    _usersModel.SendEmail(student.EMAIL, "Confirmar Cuenta", cuerpo);
                }
                catch (Exception ex)
                {
                    return "Usuario creado pero hubo un error al enviar el correo de confirmación de cuenta, pongase en contacto con el administrador.";
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el estudiante:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string StudentEdit(Users student)
        {
            try
            {
                var user_email = _usersModel.UserEmail(student.ID_USER);
                //validar si el correo cambio al editar el usuario
                if (user_email != student.EMAIL)
                {
                    //valida si ya existe otro usuario con el mismo correo
                    var user_exist = _usersModel.UserExist(student);

                    if (user_exist != null)
                    {
                        return user_exist;
                    }
                }

                if (student.PASSWORD == null)
                {
                    var password = _usersModel.UserPassword(student.ID_USER);
                    student.PASSWORD = password;
                }
                else
                {
                    var passwordEncrypt = base64Encode(student.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    student.PASSWORD = passwordEncrypt;
                }
                student.CELLPHONE = student.CELLPHONE ?? "";

                student.ADDRESS = student.ADDRESS ?? "";
                student.STATE = student.STATE ?? "";
                student.CITY = student.CITY ?? "";
                student.USERNAME = student.EMAIL;
                student.ID_ROL = 3; //id de rol estudiante

                _context.ChangeTracker.Clear();
                _context.Users.Update(student);
                _context.SaveChanges();


                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar su cuenta.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string StudentDelete(Users student)
        {
            try
            {
                //just update status 
                _context.ChangeTracker.Clear();
                _context.Users.Update(student);
                _context.SaveChanges();


                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al eliminar la cuenta.");
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
