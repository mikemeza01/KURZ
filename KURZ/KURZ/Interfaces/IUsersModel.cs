using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IUsersModel
    {
        public List<Users>? UsersList();
        public Users ValidateUser(Login login);

        public string UserCreate(Users user, string host);

        public Users UserDetail(int? ID);

        public string UserEdit(Users user);

        public int UserDelete(Users user);

        public string UserPassword(int? ID);

        public string ForgotPassword(Users user, string host);

        public string UserEmail(int? ID);

        public string UserExist(Users user);

        public Users byEmail(string email);

        public Users byUserName(string username);
        public Users byID(int ID);

        public string confirmAccount(Users user);

        public string forgotPasswordConfirmation(Users user);

        public string base64Encode(string sData);

        public string base64Decode(string sData);

        public string CreateToken(int tamanno);

        public string CreatePasswordTemporary(int tamanno);

        public string ArmarHTMLCC(Users datos, String host);

        public void SendEmail(string correoDestino, string asunto, string cuerpoCorreo);
    }
}
