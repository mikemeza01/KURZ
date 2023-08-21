using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string email)
        {
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string email)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string delete)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
