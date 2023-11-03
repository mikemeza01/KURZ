using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

                StringBuilder cuerpo = new StringBuilder("");
                cuerpo.Append(student.NAME + student.LASTNAME);
                cuerpo.Append("<br>");
                cuerpo.Append("<br>");
                cuerpo.Append("Se ha creado tu cuenta en KURZ. Debes confirmar la cuenta para poder empezar a utilizar la plataforma");
                cuerpo.Append("<br>");
                cuerpo.Append("<a href='" + host + "/Authentication/ConfirmationAccount/?username=" + student.EMAIL + "&token=" + token + "'> Confirmar cuenta</a>");
                cuerpo.Append("<br>");
                cuerpo.Append("<br>");
                cuerpo.Append("Saludos cordiales,");
                cuerpo.Append("<br>");
                cuerpo.Append("KURZ");

                try
                {
                    _usersModel.SendEmail(student.EMAIL, "Confirmar Cuenta", cuerpo.ToString());
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

        public string StudentEdit(Users student_edit)
        {
            try
            {
                // Validar si el correo cambió al editar el usuario
                if (_usersModel.UserEmail(student_edit.ID_USER) != student_edit.EMAIL)
                {
                    // Validar si ya existe otro usuario con el mismo correo
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

                var student_to_edit = _usersModel.byEmail(student_edit.EMAIL);
                if (student_to_edit != null)
                {
                    student_edit.CONFIRMATION = student_to_edit.CONFIRMATION;
                    student_edit.STATUS = student_to_edit.STATUS;
                }
                // Restablecer campos predeterminados
                student_edit.USERNAME = student_edit.EMAIL;
                student_edit.CELLPHONE = null;
                student_edit.ADDRESS = null;
                student_edit.STATE = null;
                student_edit.CITY = null;
                student_edit.ID_ROL = 3; // ID de rol para estudiante


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

        public string StudentEditAccount(Users student_edit)
        {
            try
            {
                // Validar si el correo cambió al editar el usuario
                if (_usersModel.UserEmail(student_edit.ID_USER) != student_edit.EMAIL)
                {
                    // Validar si ya existe otro usuario con el mismo correo
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

                //Mantener correo confirmado al editar perfil.
                var student_to_edit = _usersModel.byEmail(student_edit.EMAIL);
                if (student_to_edit != null)
                {
                    student_edit.CONFIRMATION = student_to_edit.CONFIRMATION;
                }

                // Restablecer campos predeterminados
                student_edit.USERNAME = student_edit.EMAIL;
                student_edit.CELLPHONE = null;
                student_edit.ADDRESS = null;
                student_edit.STATE = null;
                student_edit.CITY = null;
                student_edit.ID_ROL = 3; // ID de rol para estudiante


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
