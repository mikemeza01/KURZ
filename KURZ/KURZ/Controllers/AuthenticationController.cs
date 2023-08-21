using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email)
        {
            return RedirectToAction("DashboardAdmin", "Dashboard");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
