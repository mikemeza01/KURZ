using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class TeacherModel : ITeacherModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public TeacherModel(KurzContext context)
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

        public int UserCreate(Users teacher)
        {
            try
            {
                //se encripta la clave puesta para el usuario
                if (teacher.PASSWORD != null)
                {
                    var passwordEncrypt = base64Encode(teacher.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    teacher.PASSWORD = passwordEncrypt;
                }
                teacher.CELLPHONE = "";
                teacher.PHOTO = "";
                teacher.PROFILE = "";
                teacher.ADDRESS = "";
                teacher.STATE = "";
                teacher.CITY = "";
                teacher.ID_ROL = 2; //id de rol administrador
               

                _context.Users.Add(teacher);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el usuario:");
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
    }
}
