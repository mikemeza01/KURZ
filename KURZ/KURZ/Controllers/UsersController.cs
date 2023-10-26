using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            ViewBag.mensaje = "";
            return View();
        }

        [HttpPost]
        public IActionResult Create(UsersBinding user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var request = HttpContext.Request;
                    var host = request.Host.ToUriComponent();
                    var pathBase = request.PathBase.ToUriComponent();
                    var domain = $"{request.Scheme}://{host}{pathBase}";

                    var user_create = new Users()
                    {
                        IDENTICATION = user.IDENTICATION,
                        NAME = user.NAME,
                        LASTNAME = user.LASTNAME,
                        EMAIL = user.EMAIL,
                        PASSWORD = user.PASSWORD
                    };

                    var resultado = _usersModel.UserCreate(user_create, domain);
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
                    return View();
                }
            }
            catch (Exception) {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Details(int? ID)
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

        [HttpGet]
        public IActionResult Edit(int? ID)
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

            var user_editBindign = new UsersBinding()
            {
                ID_USER = user.ID_USER,
                IDENTICATION = user.IDENTICATION,
                NAME = user.NAME,
                LASTNAME = user.LASTNAME,
                EMAIL = user.EMAIL,
                PASSWORD = user.PASSWORD,
                PASSWORD_REPEAT = user.PASSWORD,
            };

            return View(user_editBindign);
        }

        [HttpPost]
        public IActionResult Edit(UsersBinding user)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user_edit = new Users()
                    {
                        ID_USER = user.ID_USER,
                        IDENTICATION = user.IDENTICATION,
                        NAME = user.NAME,
                        LASTNAME = user.LASTNAME,
                        EMAIL = user.EMAIL,
                        PASSWORD = user.PASSWORD
                    };

                    var resultado = _usersModel.UserEdit(user_edit);
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

        [HttpGet]
        public IActionResult Delete(int? ID)
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
        public IActionResult Delete(Users user)
        {
            try
            {
                
                var resultado = _usersModel.UserDelete(user);
                if (resultado > 0)
                {
                    ViewBag.mensaje = "SUCCESS";
                    return View(user);
                }
                else
                    ViewBag.mensaje = "ERROR";
                return View(user);
                

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        
    }
}
