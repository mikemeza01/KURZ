using KURZ.Entities;
using KURZ.Helpers;
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
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private FilesHelper filesHelper = new FilesHelper();

        protected UserDetails ConvertUsers(Users user)
        {
            try
            {
                if (user != null)
                {
                    return new UserDetails
                    {
                        ADDRESS = user.ADDRESS,
                        CELLPHONE = user.CELLPHONE,
                        CITY = user.CITY,
                        CONFIRMATION = user.CONFIRMATION,
                        EMAIL = user.EMAIL,
                        IDENTICATION = user.IDENTICATION,
                        ID_COUNTRY = user.ID_COUNTRY,
                        ID_ROL = user.ID_ROL,
                        ID_USER = user.ID_USER,
                        LASTNAME = user.LASTNAME,
                        NAME = user.NAME,
                        PASSWORD = user.PASSWORD,
                        PASSWORDTEMP = user.PASSWORDTEMP,
                        PHOTO = user.PHOTO,
                        PROFILE = user.PROFILE,
                        STATE = user.STATE,
                        STATUS = user.STATUS,
                        TOKEN = user.TOKEN,
                        USERNAME = user.USERNAME,
                    };
                }

                return new UserDetails();
            }
            catch (Exception)
            {
                return new UserDetails();
            }
        }

        public TeacherController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ITeacherModel teacherModel, ICountriesModel countriesModel, IUsersModel usersModel)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _teacherModel = teacherModel;
            _countriesModel = countriesModel;
            _usersModel = usersModel;
        }


        [HttpGet]
        public IActionResult RegisterTeachers()
        {
            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;

            return View(new UsersBinding());
        }

        [HttpPost]
        public IActionResult RegisterTeachers(UsersBinding teacher)
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

                var teacher_create = new Users()
                {
                    IDENTICATION = teacher.IDENTICATION,
                    NAME = teacher.NAME,
                    LASTNAME = teacher.LASTNAME,
                    EMAIL = teacher.EMAIL,
                    PASSWORD = teacher.PASSWORD,
                    ID_COUNTRY = teacher.ID_COUNTRY
                };


                var resultado = _teacherModel.UserCreate(teacher_create, domain);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(teacher);
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
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            var user = _usersModel.byUserName(nombreusuario);

            //Convierte el Entitie de Users a uno tipo UserDetails que maneja una variable tipo byte para guardar la imagen
            var userDetail = ConvertUsers(user);

            userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);

            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;
            return View(userDetail);
        }

        [HttpPost]
        public IActionResult Edit(UserDetails teacher, IFormFile newProfilePicture)
        {
            try
            {
                if (newProfilePicture != null && newProfilePicture.Length > 0)
                {
                    bool deleted = filesHelper.DeleteFile(teacher.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION + "\\");

                    teacher.PHOTO = newProfilePicture.FileName;
                    filesHelper.UploadFile(teacher.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION + "\\", newProfilePicture);
                    var user_photo = filesHelper.ReadFiles(teacher.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION);
                    HttpContext.Session.Set("USER_PHOTO", user_photo);
                }

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
               
                user.IDENTICATION = teacher.IDENTICATION;
                user.PHOTO = teacher.PHOTO;

                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;

                var resultado = _teacherModel.TeacherEdit(user);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    var userDetail = ConvertUsers(user);
                    userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);

                    return View(userDetail);
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
            ClaimsPrincipal claimteacher = HttpContext.User;
            string nombreusuario = "";

            if (claimteacher.Identity.IsAuthenticated)
            {
                nombreusuario = claimteacher.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            var user = _usersModel.byUserName(nombreusuario);

            var teacher_editBindign = new UsersBindingEdit()
            {
                ID_USER = user.ID_USER,
                IDENTICATION = user.IDENTICATION,
                NAME = user.NAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
                PASSWORD = user.PASSWORD,
                PASSWORD_REPEAT = user.PASSWORD,
                STATE= user.STATE,
                CITY= user.CITY,
                CELLPHONE = user.CELLPHONE,
                ADDRESS= user.ADDRESS,
                STATUS = user.STATUS
            };

            return View(teacher_editBindign);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult EditAccount(UsersBindingEdit teacher)
        {
            try
            {
                ClaimsPrincipal claimteacher = HttpContext.User;
                string nombreusuario = "";

                if (claimteacher.Identity.IsAuthenticated)
                {
                    nombreusuario = claimteacher.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var user = _usersModel.byUserName(nombreusuario);

                user.IDENTICATION = teacher.IDENTICATION;
                user.CELLPHONE = teacher.CELLPHONE;
                user.ADDRESS = teacher.ADDRESS;
                user.STATE = teacher.STATE;
                user.CITY = teacher.CITY;
                user.PASSWORD= teacher.PASSWORD;
                user.EMAIL = teacher.EMAIL;

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
                    return RedirectToAction("Index", "Home");
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
