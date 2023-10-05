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

        public int TopicsCreate(Topics topic)
        {
            try
            {
                

                _context.Topics.Add(topic);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el tema:");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }
}
