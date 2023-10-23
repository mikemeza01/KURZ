using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class AdvicesModel : IAdvicesModel
    {
        private readonly KurzContext _context;

        public AdvicesModel(KurzContext context)
        {
            _context = context;
        }

        public List<GetAdvices_Result> GetAdvices()
        {
            try
            {
                string sql = "[dbo].[GetAdvices]";
                var results = _context.GetAdvices_Result.FromSqlRaw(sql).ToList();

                if (results.Count > 0)
                {
                    return results;
                }

                return new List<GetAdvices_Result>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de advices: " + ex.Message);
            }
        }
    }
}
