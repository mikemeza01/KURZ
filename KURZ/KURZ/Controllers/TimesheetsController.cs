using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KURZ.Controllers
{
    public class TimesheetsController : Controller
    {
        //ACCESO A LA INTERFAZ.
        private readonly ITimesheetsModel _timesheetsModel;
        private readonly IUsersModel _usersModel;


        //CREACION DEL CONTROLADOR.
        public TimesheetsController(ITimesheetsModel timesheetsModel, IUsersModel usersModel)
        {
            _timesheetsModel = timesheetsModel;
            _usersModel = usersModel;
        }


        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            var user = _usersModel.byUserName(nombreusuario);

            var datos = _timesheetsModel.TimesheetDetailbyTeacher(user.ID_USER);
            var timesheethours = _timesheetsModel.TimesheetHours();
            ViewBag.timesheethours = timesheethours;

            return View();
        }
    }
}
