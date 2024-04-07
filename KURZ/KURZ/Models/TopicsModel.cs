using KURZ.Interfaces;
using KURZ.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace KURZ.Models
{
    public class TopicsModel : ITopicsModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public TopicsModel(KurzContext context)
        {
            _context = context;
        }

        public string TopicsCreate(Topics topic)
        {
            try
            {
                _context.Topics.Add(topic);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar la subcategoría:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public List<TopicsView> TopicsList()
        {
            try
            {
                var consulta = from top in _context.Topics
                               join cat in _context.Categories on top.ID_CATEGORY equals cat.ID_CATEGORY
                               join sub in _context.SubCategories on top.ID_SUBCATEGORY equals sub.ID_SUBCATEGORY
                               select new TopicsView
                               {
                                   ID_TOPIC = top.ID_TOPIC,
                                   NAME = top.NAME,
                                   DESCRIPTION = top.DESCRIPTION,
                                   CATEGORY = cat.NAME,
                                   SUBCATEGORY = sub.NAME
                               };

                return consulta.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }

        public string TopicsEdit(Topics topic)
        {
            try
            {
                _context.Topics.Update(topic);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar el tema.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Topics TopicsDetail(int? ID)
        {
            try
            {
                var topic = _context.Topics.Find(ID);
                return topic;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo de temas: " + ex.Message);
            }
        }

        public TopicsView TopicsDetailView(int? ID)
        {
            try
            {

                var consulta = from top in _context.Topics
                               join cat in _context.Categories on top.ID_CATEGORY equals cat.ID_CATEGORY
                               join sub in _context.SubCategories on top.ID_SUBCATEGORY equals sub.ID_SUBCATEGORY
                               where top.ID_TOPIC == ID
                               select new TopicsView
                               {
                                   ID_TOPIC = top.ID_TOPIC,
                                   NAME = top.NAME,
                                   DESCRIPTION = top.DESCRIPTION,
                                   CATEGORY = cat.NAME,
                                   SUBCATEGORY = sub.NAME,
                                   STATUS = top.STATUS,
                               };


                var topic = _context.Topics.Find(ID);
                return consulta.First();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo de temas: " + ex.Message);
            }
        }

        public int TopicDelete(Topics topic)
        {
            try
            {
                _context.Topics.Remove(topic);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al eliminar el tema.");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }


        public List<Topics> TopicsByCatSub(int ID_CATEGORY, int ID_SUBCATEGORY)
        {
            try
            {

                if (ID_SUBCATEGORY == 0)
                {
                    return _context.Topics.Where(x => x.ID_CATEGORY == ID_CATEGORY).ToList();
                }
                else
                {
                    return _context.Topics.Where(x => x.ID_CATEGORY == ID_CATEGORY && x.ID_SUBCATEGORY == ID_SUBCATEGORY).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de temas: " + ex.Message);
            }
        }

        public List<TeacherTopicsView> TopicByTeacherView(int ID)
        {

            // {
            //     try
            //     {
            //         string sql = "[dbo].[TopicsByTeacherView] @ID";

            //         var param = new SqlParameter[]
            //         {
            //         new SqlParameter()
            //         {
            //             ParameterName= "@ID",
            //             SqlDbType = System.Data.SqlDbType.Int,
            //             Value = ID
            //         }
            //         };

            //         var result = _context.TopicsViews.FromSqlRaw(sql, param).AsEnumerable().ToList();



            //         return result;
            //     }
            //     catch (Exception ex)
            //     {
            //         throw new Exception("Error al obtener la lista de profesores: " + ex.Message);
            //     }
            // }


            try
            {
                var tema = _context.Topics.FirstOrDefault(t => t.ID_TOPIC == ID);

                if (tema != null)
                {
                    var consulta = from pricT in _context.Price_Topics
                                   join prof in _context.Users on pricT.ID_TEACHER equals prof.ID_USER
                                   join cou in _context.Countries on prof.ID_COUNTRY equals cou.ID_COUNTRY
                                   join t in _context.Topics on pricT.ID_TOPIC equals t.ID_TOPIC
                                   join gra in _context.Grades on prof.ID_USER equals gra.ID_TEACHER into grade
                                   from gra in grade.DefaultIfEmpty()
                                   where t.ID_TOPIC == ID
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

                    var profesoresDelTema = consulta.ToList();



                    return profesoresDelTema;

                }
                else
                {
                    // Manejar el caso en que el tema no existe
                    return new List<TeacherTopicsView>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en obteniendo los datos de la vista: " + ex.Message);
            }
        }


    }
}
