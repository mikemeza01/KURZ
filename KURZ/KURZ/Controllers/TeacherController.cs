using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Teacher")]
        public IActionResult MyAccount()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Edit()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult EditAccount()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Advice()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
