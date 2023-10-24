using KURZ.Entities;
using KURZ.Interfaces;

namespace KURZ.Models
{
    public class CategoriesModel : ICategoriesModel
    {
        private readonly KurzContext _context;


        public CategoriesModel(KurzContext context)
        {
            _context = context;
        }

        public List<Categories> CategoriesList()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }
    }
}
