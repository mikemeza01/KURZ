using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KURZ.Models
{
    public class UsersModel : IUsersModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        private readonly IConfiguration _configuration;
        private IHostEnvironment _hostingEnvironment;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public UsersModel(KurzContext context, IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Users>? UsersList()
        {
            try
            {
                return _context.Users.Where(x => x.ID_ROL == 1).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
            }
        }
        public Users ValidateUser(Login login)
        {
            var email_login = login.EMAIL;
            var user_login = _context.Users.FirstOrDefault(e => e.EMAIL == email_login);
            if (user_login == null)
            {
                return null;
            }
            else
            {
                var password_decrypted = base64Decode(user_login.PASSWORD);

                if (password_decrypted == login.PASSWORD)
                {
                    return user_login;
                }
                else
                {
                    return null;
                }
            }
        }

        public string UserCreate(Users user, string host)
        {
            try
            {
                var user_exist = UserExist(user);

                if (user_exist != null)
                {
                    return user_exist;
                }

                //se encripta la clave puesta para el usuario
                if (user.PASSWORD != null)
                {
                    var passwordEncrypt = base64Encode(user.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    user.PASSWORD = passwordEncrypt;
                }

                user.USERNAME = user.EMAIL;
                user.CELLPHONE = "";
                user.PHOTO = "";
                user.PROFILE = "";
                user.ADDRESS = "";
                user.STATE = "";
                user.CITY = "";
                user.ID_ROL = 1; //id de rol administrador
                user.ID_COUNTRY = 52; //ID de País Costa Rica, esto para ponerlo para usuarios admin
                user.CONFIRMATION = false;
                user.STATUS = true;
                user.PASSWORDTEMP = false;

                var token = CreateToken(10);

                user.TOKEN = token;

                _context.Users.Add(user);
                _context.SaveChanges();

                string cuerpo = ArmarHTMLCC(user, host);
                //StringBuilder cuerpo = new StringBuilder("");
                //cuerpo.Append(user.NAME +" "+ user.LASTNAME);
                //cuerpo.Append("<br>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("Se ha creado tu cuenta en KURZ. Debes confirmar la cuenta para poder empezar a utilizar la plataforma");
                //cuerpo.Append("<br>");
                //cuerpo.Append("<a href='"+host+ "/Authentication/ConfirmationAccount/?username=" + user.EMAIL + "&token="+ token + "'> Confirmar cuenta</a>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("<br>");
                //cuerpo.Append("Saludos cordiales,");
                //cuerpo.Append("<br>");
                //cuerpo.Append("KURZ");

                try
                {
                    //SendEmail(user.EMAIL, "Confirmar Cuenta", cuerpo.ToString());
                    SendEmail(user.EMAIL, "Confirmar Cuenta", cuerpo);
                }
                catch (Exception ex)
                {
                    return "Usuario creado pero hubo un error al enviar el correo de confirmación de cuenta, pongase en contacto con el administrador.";
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el usuario:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Users UserDetail(int? ID)
        {
            try
            {
                var user = _context.Users.Find(ID);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Users: " + ex.Message);
            }
        }

        public string UserEdit(Users user_edit)
        {
            try
            {

                var user_email = UserEmail(user_edit.ID_USER);
                //validar si el correo cambio al editar el usuario
                if (user_email != user_edit.EMAIL)
                {
                    //valida si ya existe otro usuario con el mismo correo
                    var user_exist = UserExist(user_edit);

                    if (user_exist != null)
                    {
                        return user_exist;
                    }
                }

                if (user_edit.PASSWORD == null)
                {
                    var password = UserPassword(user_edit.ID_USER);
                    user_edit.PASSWORD = password;

                }
                else
                {
                    //se encripta la clave puesta para el usuario
                    if (user_edit.PASSWORD != null)
                    {
                        var passwordEncrypt = base64Encode(user_edit.PASSWORD);
                        //se pasa a la entidad la contraseña encriptada
                        user_edit.PASSWORD = passwordEncrypt;
                    }
                }

                user_edit.USERNAME = user_edit.EMAIL;
                user_edit.CELLPHONE = "";
                user_edit.PHOTO = "";
                user_edit.PROFILE = "";
                user_edit.ADDRESS = "";
                user_edit.STATE = "";
                user_edit.CITY = "";
                user_edit.ID_ROL = 1; //id de rol administrador
                user_edit.ID_COUNTRY = 52; //ID de País Costa Rica, esto para ponerlo para usuarios admin

                _context.ChangeTracker.Clear();
                _context.Users.Update(user_edit);
                _context.SaveChanges();


                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar el usuario.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public int UserDelete(Users user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al eliminar el usuario.");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public string UserPassword(int? ID)
        {
            try
            {
                //var user_find = _context.Users.Find(ID);
                //return user_find.PASSWORD;

                var user_password = _context.Users.Where(d => d.ID_USER == ID).Select(u => u.PASSWORD).FirstOrDefault();
                return user_password;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Users: " + ex.Message);
            }
        }

        public string ForgotPassword(Users user, string host)
        {
            try
            {
                var user_by_email = _context.Users.FirstOrDefault(e => e.EMAIL == user.EMAIL);

                if (user_by_email == null)
                {
                    return "ErrorUser";
                }
                else
                {
                    string cuerpo = ArmarHTMLFP(user_by_email, host);
                    //StringBuilder cuerpo = new StringBuilder("");
                    //cuerpo.Append(user_by_email.NAME +" "+ user_by_email.LASTNAME);
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("Se ha recibido su solicitud de cambiar su contraseña, dar clic en 'Cambiar contraseña' para proceder.");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("<a href='" + host + "/Authentication/ForgotPasswordConfirmation/?username=" + user_by_email.EMAIL + "&token=" + user_by_email.TOKEN + "'>Cambiar contraseña</a>");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("Saludos cordiales,");
                    //cuerpo.Append("<br>");
                    //cuerpo.Append("KURZ");

                    try
                    {
                        SendEmail(user.EMAIL, "Reestabler contraseña", cuerpo.ToString());
                    }
                    catch (Exception ex)
                    {
                        return "Error al enviar el correo de reestabler contraseña, pongase en contacto con el administrador.";
                    }

                    return "ok";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo usuario en contraseña olvidada: " + ex.Message);
            }
        }

        public string UserEmail(int? ID)
        {
            try
            {
                //var user_find = _context.Users.Find(ID);
                //return user_find.EMAIL;

                var user_email = _context.Users.Where(d => d.ID_USER == ID).Select(u => u.EMAIL).FirstOrDefault();
                return user_email;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Users: " + ex.Message);
            }
        }

        public string UserExist(Users user)
        {
            var user_by_email = _context.Users.FirstOrDefault(e => e.EMAIL == user.EMAIL);

            if (user_by_email != null)
            {
                return "Existe un usuario ya registrado con el correo electrónico: " + user.EMAIL;
            }
            else
            {
                return null;
            }
        }

        public Users byEmail(string email)
        {
            var user_by_email = _context.Users.FirstOrDefault(e => e.EMAIL == email);

            if (user_by_email != null)
            {
                return user_by_email;
            }
            else
            {
                return null;
            }
        }

        public Users byUserName(string username)
        {
            var user = _context.Users.FirstOrDefault(e => e.USERNAME == username);
            return user;
        }

         public Users byID(int ID)
        {
            var user = _context.Users.Find(ID);
            return user;
        }

        public string confirmAccount(Users user)
        {

            try
            {
                var userValidate = _context.Users.FirstOrDefault(e => e.USERNAME == user.USERNAME);

                if (userValidate != null && userValidate.CONFIRMATION == true)
                {
                    return "confirmed";
                }

                if (userValidate != null && user.TOKEN == userValidate.TOKEN)
                {
                    userValidate.CONFIRMATION = true;
                    _context.Users.Update(userValidate);
                    _context.SaveChanges();
                    return "ok";
                }
                else
                {
                    return "errorToken";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Usuarios al confirmar cuenta: " + ex.Message);
            }


        }

        public string forgotPasswordConfirmation(Users user)
        {

            try
            {
                var userValidate = _context.Users.FirstOrDefault(e => e.USERNAME == user.USERNAME);

                if (user.TOKEN == userValidate.TOKEN)
                {

                    var passwordEncrypt = base64Encode(user.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    userValidate.PASSWORD = passwordEncrypt;
                    _context.Users.Update(userValidate);
                    _context.SaveChanges();
                    return "ok";
                }
                else
                {
                    return "errorToken";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Usuarios al confirmar cuenta: " + ex.Message);
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

        //crea un string sin caracteres especiales
        public string CreateToken(int tamanno)
        {
            const string valid = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < tamanno--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        public string CreatePasswordTemporary(int tamanno)
        {
            const string valid = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@$*%";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < tamanno--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public string ArmarHTMLCC(Users datos, String host)
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//CConfirmarCuenta.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@Nombre", datos.NAME);
            htmlArchivo = htmlArchivo.Replace("@@NombreC", $"{datos.NAME} {datos.LASTNAME}");
            htmlArchivo = htmlArchivo.Replace("@@Correo", datos.EMAIL);


            htmlArchivo = htmlArchivo.Replace("@@Link", host + "/Authentication/ConfirmationAccount/?username=" + datos.EMAIL + "&token=" + datos.TOKEN);

            return htmlArchivo;
        }

        public string ArmarHTMLFP(Users datos, String host)
        {
            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//CForgotPass.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@Name", datos.NAME);
            htmlArchivo = htmlArchivo.Replace("@@FullName", $"{datos.NAME} {datos.LASTNAME}");



            htmlArchivo = htmlArchivo.Replace("@@URL", host + "/Authentication/ForgotPasswordConfirmation/?username=" + datos.EMAIL + "&token=" + datos.TOKEN);

            return htmlArchivo;
        }

        public void SendEmail(string correoDestino, string asunto, string cuerpoCorreo)
        {
            string correoSMTP = _configuration.GetSection("Variables:correoSMTP").Value;
            string contrasennaSMTP = _configuration.GetSection("Variables:contrasennaSMTP").Value;

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(correoDestino));
            msg.From = new MailAddress(correoSMTP);
            msg.Subject = asunto;
            msg.Body = cuerpoCorreo;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(correoSMTP, contrasennaSMTP);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }

        public UserDetails ConvertUsers(Users user)
        {
            try
            {
                if (user != null)
                {
                    return new UserDetails
                    {
                        ADDRESS = user.ADDRESS,
                        CELLPHONE = user.CELLPHONE,
                        CITY = user.CITY,
                        CONFIRMATION = user.CONFIRMATION,
                        EMAIL = user.EMAIL,
                        IDENTICATION = user.IDENTICATION,
                        ID_COUNTRY = user.ID_COUNTRY,
                        ID_ROL = user.ID_ROL,
                        ID_USER = user.ID_USER,
                        LASTNAME = user.LASTNAME,
                        NAME = user.NAME,
                        PASSWORD = user.PASSWORD,
                        PASSWORDTEMP = user.PASSWORDTEMP,
                        PHOTO = user.PHOTO,
                        PROFILE = user.PROFILE,
                        STATE = user.STATE,
                        STATUS = user.STATUS,
                        TOKEN = user.TOKEN,
                        USERNAME = user.USERNAME,
                    };
                }

                return new UserDetails();
            }
            catch (Exception)
            {
                return new UserDetails();
            }
        }
    }
}
