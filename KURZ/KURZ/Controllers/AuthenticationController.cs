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
        private readonly ITeacherModel _teacherModel;
        private readonly IStudentModel _studentModel;


        //constructor del controlador
        public AuthenticationController(ILogger<AuthenticationController> logger, IUsersModel usersModel, IRolesModel rolesModel, ITeacherModel teacherModel, IStudentModel studentModel)
        {
            _logger = logger;
            _usersModel = usersModel;
            _rolesModel = rolesModel;
            _teacherModel = teacherModel;
            _studentModel = studentModel;
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
                    if (result.CONFIRMATION != true)
                    {
                        ViewBag.mensaje = "UNCONFIRMEDACCOUNT";
                        return View();
                    }
                    else if (result.STATUS != true)
                    {
                        ViewBag.mensaje = "ERRORACCOUNT";
                        return View();
                    }
                    else {
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
                        else
                        {
                            return View();
                        }
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

        [HttpGet]
        public IActionResult ConfirmationAccount(string username, string token) {
            ViewBag.username = username;
            ViewBag.token = token;
            return View();
        }

        [HttpPost]
        public IActionResult ConfirmationAccount(Users user)
        {
           
            try
            {
                var result = _usersModel.confirmAccount(user);

                if (result == "ok")
                {

                    ViewBag.mensaje = "SUCCESS";
                    return View();
                }
                else if (result == "confirmed") {
                    ViewBag.mensaje = "CONFIRMED";
                    return View();
                } else if (result == "errorToken") {
                    ViewBag.mensaje = "ERROR";
                    return View();
                }
                else
                {
                    ViewBag.mensaje = "ERRORACTIVATION";
                    return View();
                }
            }
            catch
            {
                return View("Error");
            }
        }

        public IActionResult Logout() {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return View("Login");
        }
    }
}
