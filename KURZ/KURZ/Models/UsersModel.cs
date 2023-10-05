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
                return _context.Users.ToList();
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
                if (user_login.PASSWORD == user.PASSWORD)
                {
                    return user_login;
                }
                else {
                    return null;
                }
            }
        }

        public int UserCreate(Users user)
        {
            try
            {
                //se encripta la clave puesta para el usuario
                if (user.PASSWORD != null)
                {
                    var passwordEncrypt = base64Encode(user.PASSWORD);
                    //se pasa a la entidad la contraseña encriptada
                    user.PASSWORD = passwordEncrypt;
                }
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