using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    
    public class TeacherController : Controller
    {

        private readonly ITeacherModel _teacherModel;

        public TeacherController(ITeacherModel teacherModel)
        {
            _teacherModel = teacherModel;
        }


        [HttpGet]
        public IActionResult RegisterTeachers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterTeachers(Users teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _teacherModel.UserCreate(teacher);
                    if (resultado > 0)
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(teacher);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                    return View(teacher);
                }
                else
                {
                    return View(teacher);
                }

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult MyAccount()
        {
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public IActionResult Edit()
        {
            return View();
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
