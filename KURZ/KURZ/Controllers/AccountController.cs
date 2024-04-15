using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string returnUrl)
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                //return View();
                return RedirectToAction("Login", "Authentication", new { returnUrl = returnUrl });
            }
            else {
                return RedirectToAction("Login", "Authentication", new { returnUrl = returnUrl });
            }
        }
    }
}
