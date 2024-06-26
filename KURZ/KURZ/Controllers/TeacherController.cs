﻿using KURZ.Entities;
using KURZ.Helpers;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
//using NuGet.DependencyResolver;
using System.Security.Claims;

namespace KURZ.Controllers
{

    public class TeacherController : Controller
    {

        private readonly ITeacherModel _teacherModel;
        private readonly ICountriesModel _countriesModel;
        private readonly IUsersModel _usersModel;
        private readonly IAdvicesModel _advicesModel;
        private readonly IStatusModel _statusModel;
        private readonly IGradesModel _gradesModel;
        private readonly KurzContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private FilesHelper filesHelper = new FilesHelper();

        public TeacherController(IConfiguration configuration, KurzContext context, IWebHostEnvironment hostingEnvironment, ITeacherModel teacherModel, ICountriesModel countriesModel, IUsersModel usersModel, IAdvicesModel advicesModel, IGradesModel gradesModel, IStatusModel statusModel)
        {
            _configuration = configuration;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _teacherModel = teacherModel;
            _countriesModel = countriesModel;
            _usersModel = usersModel;
            _advicesModel = advicesModel;
            _gradesModel = gradesModel;
            _statusModel = statusModel;
        }

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

        public TeacherGradesView MapGradesToTeacherGradesView(Grades grades)
        {
            var teacherGradesView = new TeacherGradesView();

            // Asignar los valores de Grades a los campos de TeacherGradesView
            teacherGradesView.ID_GRADE = grades.ID_GRADE;
            teacherGradesView.COMMENTARY = grades.COMMENTARY;
            teacherGradesView.GRADE = grades.GRADE;
            teacherGradesView.DATE_GRADE = grades.DATE_GRADE;
            teacherGradesView.ID_ADVICE = grades.ID_ADVICE;
            teacherGradesView.ID_TEACHER = grades.ID_TEACHER;
            teacherGradesView.ID_STUDENT = grades.ID_STUDENT;

            return teacherGradesView;
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
                    ViewBag.mensaje = resultado;
                return View(teacher);
                //}
                //else
                //{
                //return View(new Users());
                //}

            }
            catch (Exception ex)
            {
                var error = new Error
                {
                    Message = ex.Message,
                    Name = "Error al registrar el profesor",
                };

                return RedirectToAction("Error", "Teacher", error);
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
                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var user = _usersModel.byUserName(nombreusuario);

                if (newProfilePicture != null && newProfilePicture.Length > 0)
                {
                    bool deleted = filesHelper.DeleteFile(user.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION + "\\");

                    teacher.PHOTO = newProfilePicture.FileName;
                    filesHelper.UploadFile(teacher.PHOTO, _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION + "\\", newProfilePicture);
                    var user_photo = filesHelper.ReadFiles(teacher.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + teacher.IDENTICATION);
                    HttpContext.Session.Set("USER_PHOTO", user_photo);
                }

                user.NAME = teacher.NAME;
                user.LASTNAME = teacher.LASTNAME;
                user.PROFILE = teacher.PROFILE;
                user.ID_COUNTRY = teacher.ID_COUNTRY;

                user.IDENTICATION = teacher.IDENTICATION;
                user.PHOTO = teacher.PHOTO;
                user.PASSWORD = null;

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
                STATE = user.STATE,
                CITY = user.CITY,
                CELLPHONE = user.CELLPHONE,
                ADDRESS = user.ADDRESS,
                STATUS = user.STATUS
            };
            ViewBag.USER_ID = user.ID_USER;
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
                user.PASSWORD = teacher.PASSWORD;
                user.EMAIL = teacher.EMAIL;

                var resultado = _teacherModel.TeacherEdit(user);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";

                    return View(teacher);
                }
                else if (resultado != "ok" && resultado != "error")
                {
                    ViewBag.mensaje = resultado;
                    ViewBag.error = "ERROR";
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
        [HttpGet]
        public IActionResult DeleteAccount(int? ID)
        {
            if (ID == null)
            {
                ViewData["Error"] = 1;
                return View();
            }
            var user = _usersModel.UserDetail(ID);
            if (user == null)
            {
                ViewData["Error"] = 2;
                return View();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteAccount(Users user)
        {

            try
            {
                var user_edit = _usersModel.UserDetail(user.ID_USER);
                user_edit.STATUS = false;

                var resultado = _teacherModel.TeacherDelete(user_edit);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View();
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
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);

            var advices = _advicesModel.GetAdvicesByTeacherId(user.ID_USER);
            return View(advices);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult AdviceById(int id)
        {
            try
            {
                var status = _statusModel.StatusList();
                ViewBag.status = status;
                var advices = _advicesModel.GetAdvicesById(id);

                return View(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Calendar()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeacherRate()
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
                var data = _gradesModel.TeacherGradesByUser(user.ID_USER);


                return View(data);

            }
            catch (Exception)
            {

                throw;
            }
        }//TeacherRate

        // Acción para mostrar la página de calificación

        [HttpGet]
        public IActionResult RateTeacher(int ID)
        {
            try
            {
                var advice = _advicesModel.GetAdvicesById(ID);
                if (advice == null)
                {
                    return View("Error");
                }

                // Crear funcion para validar si existe una calificacion en la asesoria.
                //    var checkrating = _gradesModel.GetGradeByID(ID);

                var teacherGradesView = new TeacherGradesView
                {
                    ID_ADVICE = advice.ID_ADVICE,

                    TeacherName = advice.TEACHERNAME,
                    Topic = advice.TOPICNAME,
                    DATE_GRADE = advice.DATE_UPDATE,
                    Status = advice.STATUSNAME,
                };

                return View(teacherGradesView);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Acción para enviar la calificación del profesor
        [HttpPost]
        public IActionResult RateTeacher(int IDADVICE, int ratingValue, string commentary)
        {

            try
            {
                var advice = _advicesModel.GetAdvicesById(IDADVICE);
                if (advice == null)
                {
                    return View("Error");
                }

                int idteacher = advice.IDTEACHER;
                int Idstudent = advice.IDSTUDENT;

                var teacher = _usersModel.byID(idteacher);
                var student = _usersModel.byID(Idstudent);


                var resultado = _teacherModel.SendRating(teacher.ID_USER, student.ID_USER, ratingValue, commentary, IDADVICE);

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    var teacherRate = new TeacherGradesView
                    {
                        ID_ADVICE = advice.ID_ADVICE,
                        TeacherName = advice.TEACHERNAME,
                        Topic = advice.TOPICNAME,
                        DATE_GRADE = advice.DATE_UPDATE,
                        Status = advice.STATUSNAME,
                    };

                    return View(teacherRate);
                }
                else if (resultado != "ok" && resultado != "error")
                {
                    ViewBag.mensaje = resultado;
                    ViewBag.error = "ERROR";
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

        public IActionResult TeacherAccount(int ID)
        {
            // Obtener el usuario por su ID
            var user = _usersModel.byID(ID);

            // Verificar si el usuario existe
            if (user == null)
            {
                // Manejar el caso en que el usuario no exista, por ejemplo, redirigiendo a una página de error
                return RedirectToAction("Error", "Home");
            }

            var consulta = from prof in _context.Users
                           join cou in _context.Countries on prof.ID_COUNTRY equals cou.ID_COUNTRY
                           join gra in _context.Grades on prof.ID_USER equals gra.ID_TEACHER into grade
                           from gra in grade.DefaultIfEmpty()
                           where prof.ID_USER == ID
                           select new TeacherAccountView
                           {
                               NAME = prof.NAME,
                               PHOTO = prof.PHOTO,
                               PROFILE = prof.PROFILE,
                               Country = cou.NAME,
                               EMAIL = prof.EMAIL,
                               AvgRating = gra != null ? gra.AverageRating : 5
                           };
            // Conversion de objeto.
            var userDetail = _usersModel.ConvertUsers(user);

            //Obtener la foto del Usuario.
            userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);
            ViewBag.user_photo = userDetail.ProfilePicture;

            var teacherviewdetail = consulta.FirstOrDefault();

            // Pasar el usuario a la vista
            return View(teacherviewdetail);
        }

        public ActionResult Error(Error error)
        {

            return View(error);
        }

    }
}
