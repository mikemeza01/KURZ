using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ICategoriesModel
    {
        public List<Categories> CategoriesList();

        public string CreateCategory(Categories category);

        public string CategoryEdit(Categories category);

        public Categories CategoryDetail(int? ID);

        public int CategoryDelete(Categories category);
       public string GetCategoryNameById(int? ID);
    }
}
