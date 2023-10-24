using KURZ.Entities;
using KURZ.Interfaces;

namespace KURZ.Models
{
    public class SubCategoriesModel : ISubCategoriesModel
    {
        private readonly KurzContext _context;


        public SubCategoriesModel(KurzContext context)
        {
            _context = context;
        }

        public List<SubCategories> SubCategoriesList()
        {
            try
            {
                return _context.SubCategories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de subcategorías: " + ex.Message);
            }
        }
    }
}
