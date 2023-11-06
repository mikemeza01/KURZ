using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface IAdvicesModel
    {
        public List<GetAdvices_Result> GetAdvices();
        public GetAdvicesById_Result GetAdvicesById(int id);
    }
}
