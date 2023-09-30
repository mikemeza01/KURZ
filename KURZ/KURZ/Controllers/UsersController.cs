using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly IUsersModel _usersModel;

        public UsersController(IUsersModel usersModel)
        {
            _usersModel = usersModel;
        }

        public IActionResult Index()
        {
            var datos = _usersModel.UsersList();
            return View(datos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = _usersModel.UserCreate(user);
                    if (resultado > 0)
                    {
                        ViewBag.mensaje = "SUCCESS";
                        return View(user);
                    }
                    else
                        ViewBag.mensaje = "ERROR";
                        return View(user);
                }
                else {
                    return View(user);
                }
                    
            }
            catch (Exception) {
                return View("Error");
            }
        }


        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string email)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string delete)
        {
            return RedirectToAction(nameof(Index));
        }

        
    }
}
