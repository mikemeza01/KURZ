using KURZ.Interfaces;
using KURZ.Entities;
using Microsoft.EntityFrameworkCore;

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
    }
}
