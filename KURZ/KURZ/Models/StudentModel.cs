using System.Text;
using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


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

                // Restablecer campos predeterminados
                student_edit.CELLPHONE = null;
                student_edit.ADDRESS = null;
                student_edit.STATE = null;
                student_edit.CITY = null;
                student_edit.ID_ROL = 3; // ID de rol para estudiante

                //if (photoFile != null && photoFile.Length > 0)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        photoFile.CopyTo(memoryStream);
                //        student_edit.PHOTO = Convert.ToBase64String(memoryStream.ToArray());
                //    }
                //}
                //else
                //{
                //    // Si no se proporcionó una imagen, asigna la imagen predeterminada
                //   // student_edit.PHOTO = 
                //}

                // Restablecer otros campos predeterminados
                student_edit.CELLPHONE = null;
                student_edit.ADDRESS = null;
                student_edit.STATE = null;
                student_edit.CITY = null;
                student_edit.ID_ROL = 3; // ID de rol para estudiante

                _context.Update(student_edit);
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
