using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IUsersModel
    {
        public List<Users>? UsersList();
        public Users ValidateUser(Users user);

        public int UserCreate(Users user);

        public Users UserDetail(int? ID);

        public int UserEdit(Users user);

        public int UserDelete(Users user);

        public string UserPassword(int? ID);

        public string base64Encode(string sData);

        public string base64Decode(string sData);
    }
}
