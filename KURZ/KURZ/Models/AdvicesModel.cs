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

        public List<GetAdvicesByTeacherId_Result> GetAdvicesByStudentId(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesByStudentId] @IdStudent";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@IdStudent",
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

        public TeacherTopicsView TopicTeacherDetails(int ID_TEACHER, int ID_TOPIC)
        {
            try
            {
                var consulta = from pricT in _context.Price_Topics
                               join prof in _context.Users on pricT.ID_TEACHER equals ID_TEACHER
                               join cou in _context.Countries on prof.ID_COUNTRY equals cou.ID_COUNTRY
                               join t in _context.Topics on pricT.ID_TOPIC equals t.ID_TOPIC
                               join gra in _context.Grades on prof.ID_USER equals gra.ID_TEACHER into grade
                               from gra in grade.DefaultIfEmpty()
                               where t.ID_TOPIC == ID_TOPIC && prof.ID_USER == ID_TEACHER
                               select new TeacherTopicsView
                               {
                                   ID_TOPIC = t.ID_TOPIC,
                                   ID_TEACHER = prof.ID_USER,
                                   TeacherName = prof.NAME,
                                   TeacherCountry = cou.NAME,
                                   Price = pricT.PRICE,
                                   NAME = t.NAME,
                                   AvgGrade = gra != null ? gra.AverageRating.GetValueOrDefault(5) : 5
                               };

                var TopicTeacherDetail = consulta.First();

                return TopicTeacherDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en obteniendo los datos del profesor y tema para solicitar asesoría: " + ex.Message);
            }
        }
    }
}
