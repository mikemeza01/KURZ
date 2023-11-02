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
            var subcategories = _SubcategoriesModel.SubCategoriesList();
            ViewBag.Subcategories = subcategories;  
            return View();
        }
        [HttpPost]
        public IActionResult Create(Topics topics)
        {
            //LLamada a la lista de datos de los topics.
            return View();
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
        public IActionResult DeleteCategorie(Categories category)
        {
            try
            {

                var resultado = _categoriesModel.CategoryDelete(category);
                if (resultado > 0)
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
        }

        [HttpGet]
        public IActionResult EditTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditTopic(string name)
        {
            return RedirectToAction(nameof(CategoriesList));
        }

        [HttpGet]
        public IActionResult DeleteTopic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteTopic(string delete)
        {
            return RedirectToAction(nameof(CategoriesList));
        }


    }
}
