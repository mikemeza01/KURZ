using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IUsersModel
    {
        public List<Users>? UsersList();
        public Users ValidateUser(Users user);

        public int UserCreate(Users user);

        public string base64Encode(string sData);

        public string base64Decode(string sData);
    }
}
