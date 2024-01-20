using KURZ.Entities;
using KURZ.Interfaces;

namespace KURZ.Models
{
    public class StatusModel : IStatusModel
    {
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public StatusModel(KurzContext context)
        {
            _context = context;
        }

        public List<Status> StatusList()
        {
            try
            {
                return _context.Status.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los estados: " + ex.Message);
            }
        }
    }
}
