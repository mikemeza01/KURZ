﻿using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class UsersModel : IUsersModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public UsersModel(KurzContext context)
        {
            _context = context;
        }

        public List<Users>? UsersList()
        {
            try
            {
                return _context.Users.Where(x=> x.ID_ROL == 1).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
            }
        }
        public Users ValidateUser(Users user) {
            var email_login = user.EMAIL;
            var user_login = _context.Users.FirstOrDefault(e => e.EMAIL == email_login);
            if (user_login == null)
            {
                return null;
            }
            else {
                var password_decrypted = base64Decode(user_login.PASSWORD);

                if (password_decrypted == user.PASSWORD)
                {
                    return user_login;
                }
                else {
                    return null;
                }
            }
        }

        public string UserCreate(Users user)
        {
            try
            {
                var user_exist = UserExist(user);

                if (user_exist != null) {
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

                _context.Users.Add(user);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el usuario:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Users UserDetail(int? ID) {
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

        public string UserEdit(Users user_edit) {
            try
            {
                

                //validar si el correo cambio al editar el usuario
                if (UserEmail(user_edit.ID_USER) != user_edit.EMAIL) {
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
                else {
                    //se encripta la clave puesta para el usuario
                    if (user_edit.PASSWORD != null)
                    {
                        var passwordEncrypt = base64Encode(user_edit.PASSWORD);
                        //se pasa a la entidad la contraseña encriptada
                        user_edit.PASSWORD = passwordEncrypt;
                    }
                }

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

        public int UserDelete(Users user) {
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
                var user_find = _context.Users.Find(ID);
                return user_find.PASSWORD;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Users: " + ex.Message);
            }
        }

        public string UserEmail(int? ID)
        {
            try
            {
                var user_find = _context.Users.Find(ID);
                return user_find.EMAIL;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Users: " + ex.Message);
            }
        }

        public string UserExist(Users user) {
            var user_by_email = _context.Users.FirstOrDefault(e => e.EMAIL == user.EMAIL);
            
            if (user_by_email != null)
            {
                return "Existe un usuario ya registrado con el correo electrónico: " + user.EMAIL;
            }
            else {
                return null;
            }
        }

        public Users byUserName(string username)
        {
            var user = _context.Users.FirstOrDefault(e => e.USERNAME == username);
            return user;
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
