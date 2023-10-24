using KURZ.Entities;
using KURZ.Interfaces;
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
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategorie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategorie(Topics topics)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var resultado = _topicsModel.TopicsCreate(topics);
                    if (resultado > 0)
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(topics);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(topics);
                }
                else
                {
                    return View(topics);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }

            //return RedirectToAction(nameof(CategoriesList));
        }

        [HttpGet]
        public IActionResult EditCategorie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditCategorie(string name)
        {
            return RedirectToAction(nameof(CategoriesList));
        }

        [HttpGet]
        public IActionResult DeleteCategorie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCategorie(string delete)
        {
            return RedirectToAction(nameof(CategoriesList));
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
