using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KURZ.Models
{
    public class PriceTopicModel : IPriceTopicModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public PriceTopicModel(KurzContext context)
        {
            _context = context;
        }


        public List<Price_Topics_Teacher> Price_Topics_Teacher(int ID_TEACHER)
        {
            try
            {
                var consulta = from prt in _context.Price_Topics
                               join use in _context.Users on prt.ID_TEACHER equals ID_TEACHER
                               join top in _context.Topics on prt.ID_TOPIC equals top.ID_TOPIC
                               where use.ID_USER == ID_TEACHER
                               select new Price_Topics_Teacher
                               {
                                   ID_PRICE_TOPIC = prt.ID_PRICE_TOPIC,
                                   PRICE = prt.PRICE,
                                   TEACHER_NAME = use.NAME + ' ' + use.LASTNAME,
                                   TOPIC_NAME = top.NAME
                               };

                return consulta.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }

        public List<Price_Topics> Price_Topics_Teacher_by_Topic(int ID_TEACHER, int ID_TOPIC)
        {
            try
            {

                var consulta = from prt in _context.Price_Topics
                               where prt.ID_TEACHER == ID_TEACHER && prt.ID_TOPIC == ID_TOPIC
                               select new Price_Topics
                               {
                                   ID_PRICE_TOPIC = prt.ID_PRICE_TOPIC,
                                   PRICE = prt.PRICE,
                                   ID_TEACHER = prt.ID_TEACHER,
                                   ID_TOPIC = prt.ID_TOPIC
                               };

                return consulta.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar si ya el profesor agrego el tema a impartir: " + ex.Message);
            }
        }

        public string CreateTopicTeacher(Price_Topics Price_Topic) {
            try
            {

                var topic_teacher_list = Price_Topics_Teacher_by_Topic(Price_Topic.ID_TEACHER, Price_Topic.ID_TOPIC);

                if (topic_teacher_list.Count == 0)
                {
                    _context.Price_Topics.Add(Price_Topic);
                    _context.SaveChanges();

                    return "ok";
                }
                else {;
                    return "exists";
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el tema al profesor:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Price_Topics TopicTeacherDetail(int? ID)
        {
            try
            {
                var price_topic = _context.Price_Topics.Find(ID);
                return price_topic;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo de Temas Profesor: " + ex.Message);
            }
        }

        public string EditTopicTeacher(Price_Topics Price_Topic)
        {
            try
            {
                _context.Price_Topics.Update(Price_Topic);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar el precio del tema del profesor.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public int DeleteTopicTeacher(Price_Topics Price_Topic)
        {
            try
            {
                _context.Price_Topics.Remove(Price_Topic);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error el tema a impartir.");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
