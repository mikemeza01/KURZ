using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using NuGet.DependencyResolver;
using System.Security.Claims;

namespace KURZ.Controllers
{

    public class TeacherController : Controller
    {

        private readonly ITeacherModel _teacherModel;
        private readonly ICountriesModel _countriesModel;
        private readonly IUsersModel _usersModel;
        public TeacherController(ITeacherModel teacherModel, ICountriesModel countriesModel, IUsersModel usersModel)
        {
            _teacherModel = teacherModel;
            _countriesModel = countriesModel;
            _usersModel = usersModel;
        }


        [HttpGet]
        public IActionResult RegisterTeachers()
        {
            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;

            return View(new Users());
        }

        [HttpPost]
        public IActionResult RegisterTeachers(Users teacher)
        {
            try
            {
                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;
                //if (ModelState.IsValid)
                //{
                var request = HttpContext.Request;
                var host = request.Host.ToUriComponent();
                var pathBase = request.PathBase.ToUriComponent();
                var domain = $"{request.Scheme}://{host}{pathBase}";
                var resultado = _teacherModel.UserCreate(teacher, domain);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return RedirectToAction("Login" , "Authentication");
                }
                else if (resultado != "ok" && resultado != "error")
                {
                    ViewBag.mensaje = resultado;
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(teacher);
                //}
                //else
                //{
                //return View(new Users());
                //}

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult MyAccount()
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";
            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;
            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);
            return View(user);
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Edit()
        {
            //HttpContext.Session.SetString("USER", result.ID_USER.ToString());

            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);
            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(Users teacher)
        {
            try
            {
                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var user = _usersModel.byUserName(nombreusuario);

                user.NAME = teacher.NAME; 
                user.LASTNAME = teacher.LASTNAME;
                user.PROFILE = teacher.PROFILE;
                user.ID_COUNTRY = teacher.ID_COUNTRY;
                user.STATUS = teacher.STATUS;
                user.EMAIL= teacher.EMAIL;  
                user.IDENTICATION = teacher.IDENTICATION;
                user.USERNAME = teacher.EMAIL;
                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;

                var resultado = _teacherModel.TeacherEdit(user);

                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(teacher);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(teacher);
                }

            catch (Exception)
            {
                return View("Error");
            }
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

        [HttpPost]
        public IActionResult DeleteAccount(bool deleteAccount)
        {

            try
            {
                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var user = _usersModel.byUserName(nombreusuario);
                user.STATUS = false;
                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;

                var resultado = _teacherModel.TeacherEdit(user);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return RedirectToAction("Index","Home");
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View();
            }

            catch (Exception)
            {
                return View("Error");
            }
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
