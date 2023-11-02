using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentModel _studentModel;
        private readonly IUsersModel _usersModel;
        private readonly ICountriesModel _countriesModel;

        public StudentController(IStudentModel studentModel, IUsersModel usersModel, ICountriesModel countriesModel)
        {
            _studentModel = studentModel;
            _usersModel = usersModel;
            _countriesModel = countriesModel;
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

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);


            ViewData["nombreUsuario"] = user.NAME + " " + user.LASTNAME;

            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Edit()
        {
            string nombreusuario = User.Identity.Name;
            var user = _usersModel.byUserName(nombreusuario);

            return View(user);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult StudentEdit(Users student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _studentModel.StudentEdit(student);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return RedirectToAction("MyAccount", "Student");
                    }
                    else if (resultado != "ok" && resultado != "error")
                    {
                        ViewBag.mensaje = resultado;
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(student);
                }
                else
                {
                    return RedirectToAction("MyAccount", "Student");
                    //return View(student);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Student")]
        public IActionResult DeleteAccount()
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var user = _usersModel.byUserName(nombreusuario);

            return View(user);
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

        //[HttpPost]
        //public async Task<IActionResult> CargarImagen(IFormFile photo)
        //{
        //    if (photo != null && photo.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await photo.CopyToAsync(memoryStream);
        //            var image = new Image
        //            {
        //                FileName = image.FileName,
        //                ContentType = image.ContentType,
        //                Data = memoryStream.ToArray(),
                        
        //            };

        //            // Guarda la imagen en la base de datos o el sistema de archivos, según tu configuración.
        //            _con.Add(image);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    return RedirectToAction("Index"); // Redirecciona a la página principal o a donde sea necesario.
        //}
    }
}
