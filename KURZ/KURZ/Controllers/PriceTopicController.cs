using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZoomNet.Models;

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
        public PriceTopicController(IPriceTopicModel priceTopicsModel, IUsersModel usersModel, ICategoriesModel categories, ISubCategoriesModel subcategories)
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
            var itemToRemove = categories.Single(r => r.ID_CATEGORY == 1);
            categories.Remove(itemToRemove);
            ViewBag.Categories = categories;
            ViewBag.ID_TEACHER = user.ID_USER;
            return View(datos);
        }

        [HttpPost]
        public IActionResult CreateTopicTeacher(string ID_TOPIC_NEW, string ADD_PRICE, string ID_TEACHER_NEW) {
            try
            {
                var Price_Topics = new Price_Topics();
                Price_Topics.PRICE = float.Parse(ADD_PRICE);
                Price_Topics.ID_TEACHER = int.Parse(ID_TEACHER_NEW);
                Price_Topics.ID_TOPIC = int.Parse(ID_TOPIC_NEW);

                var resultado = _priceTopicsModel.CreateTopicTeacher(Price_Topics);
                var datos = _priceTopicsModel.Price_Topics_Teacher(int.Parse(ID_TEACHER_NEW));
                var categories = _categoriesModel.CategoriesList();
                var itemToRemove = categories.Single(r => r.ID_CATEGORY == 1);
                categories.Remove(itemToRemove);
                if (resultado == "ok")
                {
                    ViewBag.Categories = categories;
                    ViewBag.ID_TEACHER = int.Parse(ID_TEACHER_NEW);
                    ViewBag.mensaje = "SUCCESS";
                    return View("TopicsListTeacher", datos);
                } else if (resultado == "exists") {
                    ViewBag.Categories = categories;
                    ViewBag.ID_TEACHER = int.Parse(ID_TEACHER_NEW);
                    ViewBag.mensaje = "EXISTS";
                    return View("TopicsListTeacher", datos);
                }
                else
                {
                    ViewBag.Categories = categories;
                    ViewBag.ID_TEACHER = int.Parse(ID_TEACHER_NEW);
                    ViewBag.mensaje = "ERROR";
                    return View("TopicsListTeacher", datos);
                }
                    

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult EditTopicTeacher(int ID) {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var price_topic = _priceTopicsModel.TopicTeacherDetail(ID);
            if (price_topic == null)
            {
                ViewData["Error"] = 2;
                return View();
            }

            return View(price_topic);
        }


        [HttpPost]
        public IActionResult EditTopicTeacher(string ID_PRICE_TOPIC, string PRICE, string ID_TEACHER, string ID_TOPIC)
        {
            try
            {
                var Price_Topic = new Price_Topics();
                Price_Topic.ID_PRICE_TOPIC = int.Parse(ID_PRICE_TOPIC);
                Price_Topic.PRICE = float.Parse(PRICE);
                Price_Topic.ID_TEACHER = int.Parse(ID_TEACHER);
                Price_Topic.ID_TOPIC = int.Parse(ID_TOPIC);


                var resultado = _priceTopicsModel.EditTopicTeacher(Price_Topic);
                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESSEDIT";
                    return View(Price_Topic);
                }
                else
                    ViewBag.mensaje = "ERROREDIT";
                return View(Price_Topic);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult DeleteTopicTeacher(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var price_topic = _priceTopicsModel.TopicTeacherDetail(ID);
            if (price_topic == null)
            {
                ViewData["Error"] = 2;
                return View();
            }
            return View(price_topic);
        }

        [HttpPost]
        public IActionResult DeleteTopicTeacher(string ID_PRICE_TOPIC)
        {
            try
            {

                var Price_Topic = new Price_Topics();
                Price_Topic.ID_PRICE_TOPIC = int.Parse(ID_PRICE_TOPIC);

                var resultado = _priceTopicsModel.DeleteTopicTeacher(Price_Topic);
                if (resultado > 0)
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(Price_Topic);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(Price_Topic);


            }
            catch (Exception)
            {
                return View("Error");
            }
        }

    }
}
