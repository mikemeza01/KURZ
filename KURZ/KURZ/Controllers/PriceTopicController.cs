using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class PriceTopicController : Controller
    {

        //ACCESO A LA INTERFAZ.
        private readonly IPriceTopicModel _priceTopicsModel;
        private readonly IUsersModel _usersModel;
        private readonly ICategoriesModel _categoriesModel;
        private readonly ISubCategoriesModel _SubcategoriesModel;

        //CREACION DEL CONTROLADOR.
        public PriceTopicController(IPriceTopicModel priceTopicsModel, IUsersModel usersModel, ICategoriesModel categories, ISubCategoriesModel subcategories )
        {
            _priceTopicsModel = priceTopicsModel;
            _usersModel = usersModel;
            _categoriesModel = categories;
            _SubcategoriesModel = subcategories;
        }


        [Authorize(Roles = "Teacher")]
        
        public IActionResult TopicsListTeacher()
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);


            //LLamada a la lista de datos de los topics.
            var datos = _priceTopicsModel.Price_Topics_Teacher(user.ID_USER);
            var categories = _categoriesModel.CategoriesList();
            ViewBag.Categories = categories;
            return View(datos);
        }
    }
}
