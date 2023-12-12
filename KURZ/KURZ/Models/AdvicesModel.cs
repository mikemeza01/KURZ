using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.Data.SqlClient;
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

        public GetAdvicesById_Result GetAdvicesById(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesById] @ID_ADVICE";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@ID_ADVICE",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };

                var result = _context.GetAdvicesById_Result.FromSqlRaw(sql, param).ToList();

                if (result.Count > 0)
                {
                    return result.FirstOrDefault();
                }

                return new GetAdvicesById_Result();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el advice seleccionado: " + ex.Message);
            }
        }

        public List<GetAdvicesByTeacherId_Result> GetAdvicesByTeacherId(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesByTeacherId] @IdTeacher";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@IdTeacher",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };

                var result = _context.GetAdvicesByTeacherId_Result.FromSqlRaw(sql, param).ToList();

                if (result.Count > 0)
                {
                    return result;
                }

                return new List<GetAdvicesByTeacherId_Result>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de advices: " + ex.Message);
            }
        }
    }
}
