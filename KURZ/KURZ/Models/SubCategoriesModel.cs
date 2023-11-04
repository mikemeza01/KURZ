using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class SubCategoriesModel : ISubCategoriesModel
    {
        private readonly KurzContext _context;


        public SubCategoriesModel(KurzContext context)
        {
            _context = context;
        }

        public List<SubCategoriesView> SubCategoriesList()
        {
            try
            {
                var consulta = from cat in _context.Categories
                               join sub in _context.SubCategories on cat.ID_CATEGORY equals sub.ID_CATEGORY
                               select new SubCategoriesView
                               {
                                   ID_SUBCATEGORY = sub.ID_SUBCATEGORY,
                                   NAME = sub.NAME,
                                   DESCRIPTION = sub.DESCRIPTION,
                                   CATEGORY = cat.NAME
                               };

                return consulta.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de subcategorías: " + ex.Message);
            }
        }

        public string CreateSubcategory(SubCategories subcategory)
        {
            try
            {
                _context.SubCategories.Add(subcategory);
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

        public string SubcategoryEdit(SubCategories subcategory)
        {
            try
            {
                _context.SubCategories.Update(subcategory);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar la subcategoría.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public SubCategories SubcategoryDetail(int? ID)
        {
            try
            {
                var subcategory = _context.SubCategories.Find(ID);
                return subcategory;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo subcategorías: " + ex.Message);
            }
        }

        public int SubcategoryDelete(SubCategories subcategory)
        {
            try
            {
                _context.SubCategories.Remove(subcategory);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al eliminar la subcategoría.");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public List<SubCategories> SubcategoriesByCategory(int id_category)
        {
            try
            {
                return _context.SubCategories.Where(x => x.ID_CATEGORY == id_category).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de subcategorías: " + ex.Message);
            }
        }
    }
}
