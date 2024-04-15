using System.Data.Common;
using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace KURZ.Controllers
{
    public class TopicsController : Controller
    {
        //ACCESO A LA INTERFAZ.
        private readonly ITopicsModel _topicsModel;
        private readonly ICategoriesModel _categoriesModel;
        private readonly ISubCategoriesModel _SubcategoriesModel;

        //CREACION DEL CONTROLADOR.
        public TopicsController(ITopicsModel topicsModel, ICategoriesModel categoriesModel, ISubCategoriesModel subcategoriesModel)
        {
            _topicsModel = topicsModel;
            _categoriesModel = categoriesModel;
            _SubcategoriesModel = subcategoriesModel;
        }

        public IActionResult Index()
        {
            //LLamada a la lista de datos de los topics.
            var datos = _topicsModel.TopicsList();
            return View(datos);
        }
        public IActionResult Create()
        {
            var categories = _categoriesModel.CategoriesList();
            ViewBag.Categories = categories;
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Topics topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _categoriesModel.CategoriesList();
                    ViewBag.categories = categories;

                    var resultado = _topicsModel.TopicsCreate(topic);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(topic);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(topic);
                }
                else
                {
                    return View(topic);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public List<SubCategories> SubcategoriesByCategory(SubCategories subcategory)
        {
            var subcategories = _SubcategoriesModel.SubcategoriesByCategory(subcategory.ID_CATEGORY);
            return subcategories;
        }

        [HttpPost]
        public List<Topics> TopicsByCatSub(int ID_CATEGORY, int ID_SUBCATEGORY)
        {
            var topics = _topicsModel.TopicsByCatSub(ID_CATEGORY, ID_SUBCATEGORY);
            return topics;
        }

        [HttpGet]
        public IActionResult CategoriesList()
        {
            var categories = _categoriesModel.CategoriesList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategorie()
        {
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategorie(Categories category)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var resultado = _categoriesModel.CreateCategory(category);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(category);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(category);
                }
                else
                {
                    return View(category);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }

            //return RedirectToAction(nameof(CategoriesList));
        }

        [HttpGet]
        public IActionResult EditCategorie(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var user = _categoriesModel.CategoryDetail(ID);
            if (user == null)
            {
                ViewData["Error"] = 2;
                return View();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditCategorie(Categories category)
        {
            try
            {

                var resultado = _categoriesModel.CategoryEdit(category);
                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(category);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(category);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult DeleteCategorie(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var category = _categoriesModel.CategoryDetail(ID);
            if (category == null)
            {
                ViewData["Error"] = 2;
                return View();
            }
            return View(category);
        }

        [HttpPost]
        public String DeleteCategorie(Categories category)
        {
            try
            {

                var resultado = _categoriesModel.CategoryDelete(category);
                if (resultado > 0)
                {
                    ViewBag.mensaje = "SUCCESS";
                    return "SUCCESS";
                }
                else
                    ViewBag.mensaje = "ERROR";
                return "ERROR";


            }
            catch (Exception)
            {
                return "ERROR";
            }
        }


        [HttpGet]
        public IActionResult SubCategoriesList()
        {
            var subcategories = _SubcategoriesModel.SubCategoriesList();
            return View(subcategories);
        }

        [HttpGet]
        public IActionResult CreateSubcategorie()
        {
            var categories = _categoriesModel.CategoriesList();
            ViewBag.categories = categories;
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public IActionResult CreateSubcategorie(SubCategories subcategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _categoriesModel.CategoriesList();
                    ViewBag.categories = categories;

                    var resultado = _SubcategoriesModel.CreateSubcategory(subcategory);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(subcategory);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(subcategory);
                }
                else
                {
                    return View(subcategory);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }

            //return RedirectToAction(nameof(CategoriesList));
        }

        [HttpGet]
        public IActionResult EditSubcategorie(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var subcategory = _SubcategoriesModel.SubcategoryDetail(ID);
            if (subcategory == null)
            {
                ViewData["Error"] = 2;
                return View();
            }

            var categories = _categoriesModel.CategoriesList();
            ViewBag.categories = categories;

            return View(subcategory);
        }

        [HttpPost]
        public IActionResult EditSubcategorie(SubCategories subcategory)
        {
            try
            {
                var categories = _categoriesModel.CategoriesList();
                ViewBag.categories = categories;

                if (subcategory.ID_SUBCATEGORY == 1)
                {
                    subcategory.ID_CATEGORY = 1;
                }

                var resultado = _SubcategoriesModel.SubcategoryEdit(subcategory);
                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(subcategory);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(subcategory);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult DeleteSubcategorie(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var subcategory = _SubcategoriesModel.SubcategoryDetail(ID);
            if (subcategory == null)
            {
                ViewData["Error"] = 2;
                return View();
            }
            return View(subcategory);
        }

        [HttpPost]
        public string DeleteSubcategorie(SubCategories subcategory)
        {
            try
            {

                var resultado = _SubcategoriesModel.SubcategoryDelete(subcategory);
                if (resultado > 0)
                {
                    ViewBag.mensaje = "SUCCESS";
                    return "SUCCESS";
                }
                else
                    ViewBag.mensaje = "ERROR";
                return "ERROR";


            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        [HttpPost]
        public ActionResult Search(int idCategory, int IdSubcatgory)
        {
            // Realizar la busqueda de los temas segun categoria y subcategoria.
            var temasFiltrados = _topicsModel.TopicsByCatSub(idCategory, IdSubcatgory);
            var categories = _categoriesModel.CategoriesList();

            // Obtener el nombre de la categoría seleccionada
            var selectedCategoryName = _categoriesModel.GetCategoryNameById(idCategory);  // Obtener el nombre de la categoría seleccionada


            ViewBag.categories = categories;
            ViewBag.SelectedCategoryName = selectedCategoryName;

            var temasFiltradosView = temasFiltrados.Select(t => new TopicsView
            {
                // Mapear  campos de Topics a TopicsView 
                ID_TOPIC = t.ID_TOPIC,
                NAME = t.NAME,
                DESCRIPTION = t.DESCRIPTION,
                ID_CATEGORY = t.ID_CATEGORY

            }).ToList();

            // Devolver la vista con los resultados
            return View("~/Views/Student/Advice.cshtml", temasFiltradosView);
        }




        /*[HttpGet]
        public IActionResult TopicsList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTopic(string name)
        {
            return RedirectToAction(nameof(CategoriesList));
        }*/

        [HttpGet]
        public IActionResult TopicDetails(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var topic = _topicsModel.TopicsDetailView(ID);
            if (topic == null)
            {
                ViewData["Error"] = 2;
                return View();
            }
            return View(topic);
        }

        [HttpGet]
        public IActionResult EditTopic(int ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var topic = _topicsModel.TopicsDetail(ID);
            if (topic == null)
            {
                ViewData["Error"] = 2;
                return View();
            }

            var categories = _categoriesModel.CategoriesList();
            ViewBag.categories = categories;

            return View(topic);
        }

        [HttpPost]
        public IActionResult EditTopic(Topics topic)
        {
            try
            {
                var categories = _categoriesModel.CategoriesList();
                ViewBag.categories = categories;

                /*if (topic.ID_SUBCATEGORY == 1)
                {
                    topic.ID_CATEGORY = 1;
                }*/

                var resultado = _topicsModel.TopicsEdit(topic);
                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(topic);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(topic);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /*[HttpGet]
        public IActionResult DeleteTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteTopic(string delete)
        {
            return RedirectToAction(nameof(CategoriesList));
        }*/

        public IActionResult TopicsByTeacher(int ID)
        {
            try
            {
                var topicsdetail = _topicsModel.TopicByTeacherView(ID);

                if (topicsdetail == null || topicsdetail.Count == 0)
                {
                    ViewBag.mensaje = "ERROR";
                    return View();
                }
                    return View(topicsdetail);
                


            }
            catch (Exception ex)
            {
                // Manejar el error de manera adecuada
                ViewBag.ErrorMessage = "Se produjo un error al obtener los datos: " + ex.Message;
                return View();
            }
        }

    }
}
