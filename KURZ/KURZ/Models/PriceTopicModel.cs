using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                               select new Price_Topics_Teacher
                               {
                                   ID_PRICE_TOPIC = prt.ID_PRICE_TOPIC,
                                   PRICE = prt.PRICE,
                                   TOPIC_NAME = top.NAME,
                                   TEACHER_NAME = use.NAME+' '+use.LASTNAME,
                               };

                return consulta.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }
    }
}
