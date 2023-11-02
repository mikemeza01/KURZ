using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public string CreateCategory(Categories category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar el tema:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string CategoryEdit(Categories category)
        {
            try
            {
                _context.Categories.Update(category);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar la categoría.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Categories CategoryDetail(int? ID)
        {
            try
            {
                var category = _context.Categories.Find(ID);
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Categorías: " + ex.Message);
            }
        }

        public int CategoryDelete(Categories category)
        {
            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

                return 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al eliminar la categoría.");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

    }
}
