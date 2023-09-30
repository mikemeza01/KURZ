using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Student")]
        public IActionResult MyAccount()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Edit()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult EditAccount()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Advice()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
