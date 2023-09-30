using KURZ.Interfaces;
using KURZ.Entities;

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

        public int get_num() {
            return 0;
        }
    }
}
