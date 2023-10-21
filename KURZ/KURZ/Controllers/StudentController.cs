﻿using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentModel _studentModel;
        private readonly IUsersModel _usersModel;

        public StudentController(IStudentModel studentModel, IUsersModel usersModel)
        {
            _studentModel = studentModel;
            _usersModel = usersModel;
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
                    return View(student);
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
    }
}
