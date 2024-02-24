using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ISubCategoriesModel
    {
        public List<SubCategoriesView> SubCategoriesList();

        public string CreateSubcategory(SubCategories subcategory);

        public string SubcategoryEdit(SubCategories subcategory);

        public SubCategories SubcategoryDetail(int? ID);

        public int SubcategoryDelete(SubCategories subcategory);

        public List<SubCategories> SubcategoriesByCategory(int id_category);
    }
}
