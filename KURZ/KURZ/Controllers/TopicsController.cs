using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class TopicsController : Controller
    {
        public IActionResult Index()
        {
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
        public IActionResult CreateCategorie(string name)
        {
            return RedirectToAction(nameof(CategoriesList));
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
