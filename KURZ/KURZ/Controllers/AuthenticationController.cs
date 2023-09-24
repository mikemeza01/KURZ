using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                HttpContext.Session.Clear();
                return View();
            }
            catch
            {
                return View("Error");
            }
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
