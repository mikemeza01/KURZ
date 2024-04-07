using KURZ.Entities;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System;
using System.Collections;
using System.Security.Claims;
using ZoomNet.Models;
using ZoomNet.Resources;

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
            dynamic jsonDatos = JsonConvert.DeserializeObject(datos.TIMESHEET);
            ViewBag.datos = jsonDatos;
            ViewBag.timesheethours = timesheethours;
            ViewBag.ID_TEACHER = user.ID_USER;

            return View();
        }

        [HttpPost]
        public string Update(int ID_TEACHER, string TIMESHEET) {
            try
            {
                var timesheets = new Timesheets();
                timesheets.TIMESHEET = TIMESHEET;
                timesheets.ID_TEACHER  = ID_TEACHER;

                var resultado = _timesheetsModel.Update(timesheets);
                

                if (resultado == "ok")
                {
                    ViewBag.mensaje = "SUCCESS";
                    return "SUCCESS";
                }
                else
                    ViewBag.mensaje = "ERROR";
                    return "ERROR";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }


        [HttpPost]
        public List<TimesheetData> LoadTimesheetData(int ID_TEACHER)
        {
            try
            {
                var datos = _timesheetsModel.TimesheetDetailbyTeacher(ID_TEACHER);
                dynamic jsonDatos = JsonConvert.DeserializeObject(datos.TIMESHEET);

                List<TimesheetData> items = ((JArray)jsonDatos).Select(x => new TimesheetData
                {
                    day_off = (string)x["day_off"],
                    start = (string)x["start"],
                    end = (string)x["end"],
                    breaks = x["breaks"].ToJson(),
                }).ToList();



                return items;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public List<TimesheetHours> TimesheetHours() {
            var timesheethours = _timesheetsModel.TimesheetHours();
            return timesheethours;
        }
    }
}
