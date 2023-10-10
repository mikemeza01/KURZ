using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentModel _studentModel;

        public StudentController(IStudentModel studentModel)
        {
            _studentModel = studentModel;
        }

        [HttpGet]
        public IActionResult RegisterStudents()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterStudents(Users student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _studentModel.StudentCreate(student);
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
                }
                else
                {
                    ViewBag.mensaje = "ERROR";
                    return View(student);
                }

            }
            catch(Exception)
            {
                return View("ERROR");
            }
            
        }
        [Authorize(Roles = "Student")]
        public IActionResult MyAccount()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Edit()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult Edit(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _studentModel.UserEdit(user);
                    if (resultado == "ok")
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(user);
                    }
                    else if (resultado != "ok" && resultado != "error")
                    {
                        ViewBag.mensaje = resultado;
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(user);
                }
                else
                {
                    return View(user);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public IActionResult DeleteAccount()
        {
            return View();
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
