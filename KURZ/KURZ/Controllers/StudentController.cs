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
    public class StudentController : Controller
    {
        private readonly IStudentModel _studentModel;
        private readonly IUsersModel _usersModel;
        private readonly ICountriesModel _countriesModel;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
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

        public StudentController(IStudentModel studentModel, IUsersModel usersModel, ICountriesModel countriesModel, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _studentModel = studentModel;
            _usersModel = usersModel;
            _countriesModel = countriesModel;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult RegisterStudents()
        {
            var countries = _countriesModel.CountriesList();
            ViewBag.countries = countries;

            return View(new UsersBinding());
        }

        [HttpPost]
        public IActionResult RegisterStudents(UsersBinding student)
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

                    var student_create = new Users()
                    {
                        IDENTICATION = student.IDENTICATION,
                        NAME = student.NAME,
                        LASTNAME = student.LASTNAME,
                        EMAIL = student.EMAIL,
                        PASSWORD = student.PASSWORD,
                        ID_COUNTRY = student.ID_COUNTRY
                    };


                var resultado = _studentModel.StudentCreate(student_create, domain);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(student);
                    }
                    else if (resultado != "ok" && resultado != "error")
                    {
                        ViewBag.mensaje = resultado;
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                        return View(student);
                //}
                //else
                //{
                    //ViewBag.mensaje = "ERROR";
                    //return View(student);
                //}

            }
            catch(Exception)
             {
                return View("ERROR");
            }
            
        }

        [Authorize(Roles = "Student")]
        public IActionResult MyAccount()
        {
            //DEOLVER EL NOMBRE DEL USUARIO REGISTRADO EN LA PANTALLA PRINCIPAL.
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";
            //Verificar con Michael la vista de Index de los Estudiantes.
            //var countries = _countriesModel.CountriesList();
            //ViewBag.countries = countries;
            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);
            return View(user);
        }

        [Authorize(Roles = "Student")]
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
        public IActionResult Edit(UserDetails student, IFormFile newProfilePicture)
        {
            try
            {
                if (newProfilePicture != null && newProfilePicture.Length > 0)
                {
                    bool deleted = filesHelper.DeleteFile(student.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + student.IDENTICATION + "\\");

                    student.PHOTO = newProfilePicture.FileName;
                    filesHelper.UploadFile(student.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + student.IDENTICATION + "\\", newProfilePicture);
                    var user_photo = filesHelper.ReadFiles(student.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + student.IDENTICATION);
                    HttpContext.Session.Set("USER_PHOTO", user_photo);
                }

                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var user = _usersModel.byUserName(nombreusuario);

                user.NAME = student.NAME;
                user.LASTNAME = student.LASTNAME;
                user.PROFILE = student.PROFILE;
                user.ID_COUNTRY = student.ID_COUNTRY;
                user.EMAIL = student.EMAIL;
                user.IDENTICATION = student.IDENTICATION;
                user.USERNAME = student.EMAIL;
                user.PHOTO = student.PHOTO;

                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;

                var resultado = _studentModel.StudentEdit(student);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    var userDetail = ConvertUsers(user);
                    userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);

                    return View(userDetail);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(student);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Student")]
        public IActionResult EditAccount()
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            var user = _usersModel.byUserName(nombreusuario);

            var student_editBindign = new UsersBindingEdit()
            {
                ID_USER = user.ID_USER,
                IDENTICATION = user.IDENTICATION,
                NAME = user.NAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
                PASSWORD = user.PASSWORD,
                PASSWORD_REPEAT = user.PASSWORD,
                STATUS = user.STATUS
            };

            //var countries = _countriesModel.CountriesList();
            //ViewBag.countries = countries;
            return View(student_editBindign);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult EditAccount(UsersBindingEdit student)
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

                user.NAME = student.NAME;
                user.LASTNAME = student.LASTNAME;
                user.PROFILE = student.PROFILE;
                user.ID_COUNTRY = student.ID_COUNTRY;
                user.EMAIL = student.EMAIL;
                user.IDENTICATION = student.IDENTICATION;
                user.USERNAME = student.EMAIL;
                user.PHOTO = student.PHOTO;

                //var countries = _countriesModel.CountriesList();
                //ViewBag.countries = countries;

                var resultado = _studentModel.StudentEditAccount(student);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    var userDetail = ConvertUsers(student);
                    userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);

                    return View(userDetail);
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

        
        [Authorize(Roles = "Student")]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        
        [Authorize(Roles = "Student")]
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
                var countries = _countriesModel.CountriesList();
                ViewBag.countries = countries;
                var resultado = _studentModel.StudentEditAccount(user);

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
