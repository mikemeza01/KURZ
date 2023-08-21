using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult RegisterStudents()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterStudents(string email)
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
