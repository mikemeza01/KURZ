using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        //llamado a el modelo de users
        private readonly IUsersModel _usersModel;
        private readonly IRolesModel _rolesModel;

        //constructor del controlador
        public AuthenticationController(ILogger<AuthenticationController> logger, IUsersModel usersModel, IRolesModel rolesModel)
        {
            _logger = logger;
            _usersModel = usersModel;
            _rolesModel = rolesModel;
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
        public IActionResult Login(Users user)
        {
            try
            {
                var result = _usersModel.ValidateUser(user);

                if (result != null)
                {

                    HttpContext.Session.SetString("ROL", result.ID_ROL.ToString());
                    HttpContext.Session.SetString("USER", result.ID_USER.ToString());

                    var role_name = _rolesModel.get_role_name(result.ID_ROL);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.USERNAME),
                        new Claim(ClaimTypes.Role, role_name)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    HttpContext.SignInAsync(principal);

                    if (result.ID_ROL == 1)
                    {
                        return RedirectToAction("DashboardAdmin", "Dashboard");
                    }
                    else if (result.ID_ROL == 2)
                    {
                        return RedirectToAction("MyAccount", "Teacher");
                    }
                    else if (result.ID_ROL == 3)
                    {
                        return RedirectToAction("MyAccount", "Student");
                    }
                    else {
                        return View();
                    }
                }
                else
                {
                    ViewBag.mensaje = "ERRORCREDENTIALS";
                    return View();
                }
            }
            catch
            {
                return View("Error");
            }

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(Users user)
        {
            

            return View();
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return View("Login");
        }
    }
}
