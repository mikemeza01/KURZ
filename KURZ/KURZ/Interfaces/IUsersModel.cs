using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IUsersModel
    {
        public List<Users>? UsersList();
        public Users ValidateUser(Users user);

        public string UserCreate(Users user, string host);

        public Users UserDetail(int? ID);

        public string UserEdit(Users user);

        public int UserDelete(Users user);

        public string UserPassword(int? ID);

        public string UserEmail(int? ID);

        public string UserExist(Users user);

        public Users byUserName(string username);

        public string confirmAccount(Users user);

        public string base64Encode(string sData);

        public string base64Decode(string sData);

        public string CreateToken(int tamanno);

        public string CreatePasswordTemporary(int tamanno);

        public void SendEmail(string correoDestino, string asunto, string cuerpoCorreo);
    }
}
