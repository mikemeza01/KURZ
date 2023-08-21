using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class TeacherController : Controller
    {
        [HttpGet]
        public IActionResult RegisterTeachers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterTeachers(string email)
        {
            return RedirectToAction("MyAccount");
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult EditAccount()
        {
            return View();
        }

        public IActionResult DeleteAccount()
        {
            return View();
        }

        public IActionResult Advice()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }
    }
}
