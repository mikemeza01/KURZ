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
        private readonly IAdvicesModel _advicesModel;


        //CREACION DEL CONTROLADOR.
        public TimesheetsController(ITimesheetsModel timesheetsModel, IUsersModel usersModel, IAdvicesModel  advicesModel)
        {
            _timesheetsModel = timesheetsModel;
            _usersModel = usersModel;
            _advicesModel = advicesModel;
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
            if (datos.TIMESHEET != null)
            {
                dynamic jsonDatos = JsonConvert.DeserializeObject(datos.TIMESHEET);
                ViewBag.datos = jsonDatos;
            }
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

        public List<TimesheetHours>? FilterTimesheetHoursByTeacher(int ID_TEACHER, int ID_DIA, String DATE_SELECTED)
        {
            //se obtiene el horario del profesor
            var datos = _timesheetsModel.TimesheetDetailbyTeacher(ID_TEACHER);
            //se convierte a formato JSON
            dynamic jsonDatos = JsonConvert.DeserializeObject(datos.TIMESHEET);

            //Se pasa el numero de día a numero index del día
            int indexOfday = ID_DIA - 1;

            //se hace una lista de los horarios por día
            List<TimesheetData> items = ((JArray)jsonDatos).Select(x => new TimesheetData
            {
                day_off = (string)x["day_off"],
                start = (string)x["start"],
                end = (string)x["end"],
                breaks = x["breaks"].ToJson(),
            }).ToList();

            //se obtiene el horario del día que selecciono el usuario
            //ejemplo lunes indexOfday y es 0, miercoles 2, viernes 4...
            var timesheetday = items.ElementAt(indexOfday);

            //se valida que no se un día marcado como libre, para devolver que para ese día no hay horarios disponibles.
            if (timesheetday.day_off == "0")
            {
                //se obtiene el objeto de lista de horas
                var timesheethours = _timesheetsModel.TimesheetHours();

                //se obtiene las primeras dos letras del string de hora inicio
                var starHour = timesheetday.start != null && timesheetday.start.Length > 2 ? timesheetday.start.Substring(0, 2) : "";
                //se pasa a valor entero la hora inicial
                int starHourInt = Int32.Parse(starHour);
                //se obtiene las primeras dos letras del string de hora final
                var endHour = timesheetday.end != null && timesheetday.end.Length > 2 ? timesheetday.end.Substring(0, 2) : "";
                //se pasa a valor entero la hora final
                int endHourInt = Int32.Parse(endHour);

                //se cuentan los elementos manualmente para el ciclo for y se recorra completo todos los objetos de horas
                int timesheetElements = timesheethours.Count;

                //se recorren todos los objetos de hora
                for (int i = 0; i < timesheetElements; i++)
                {
                    //se valida que la hora no este dentro del horario de hora inicio y hora final
                    if ((i < starHourInt && i < endHourInt) || (i > starHourInt && i > endHourInt))
                    {
                        //si no esta procede a eliminar el elemento de la lista comparando el ID y el index del for
                        var itemToRemove = timesheethours.SingleOrDefault(x => x.ID_TIMESHEETHOURS == i);
                        if (itemToRemove != null)
                            timesheethours.Remove(itemToRemove);
                    }
                }

                //obtienen los breaks en formato JSON
                dynamic breaks = JsonConvert.DeserializeObject(timesheetday.breaks);

                //se recorren los breaks y se asignan a una lista de Tipo TimesheetBreak con la hora de inicio y final
                List<TimesheetBreak> timesheetBreaks = ((JArray)breaks[0]).Select(x => new TimesheetBreak
                {
                    breakStart = (string)x[0],
                    breakEnd = (string)x[1],
                }).ToList();


                //Se recorren los breaks y se eliminan de los horarios disponibles se compara con hora de inicio de break
                foreach (var breakDetail in timesheetBreaks)
                {
                    //se hace la comparacion por el valor del horario y la hora inicial del break
                    var itemToRemoveBreak = timesheethours.SingleOrDefault(x => x.VALUE == breakDetail.breakStart);
                    if (itemToRemoveBreak != null)
                        timesheethours.Remove(itemToRemoveBreak);
                }

                /*
                 * En este punto ya están las horas disponibles por día según el horario que el profesor configuro.
                 * Además se quito de la lista los tiempos de descanso
                 * 
                 * Ahora se debe quitar de la lista de las horas disponibles, en las que se alla creado una asesoría a esa hora.
                 * 
                 */

                DateTime dateSelected = DateTime.Parse(DATE_SELECTED);


                var teacherAdvicesDay = _advicesModel.AdvicesforTeacherAndDate(ID_TEACHER, dateSelected);


                foreach (var teacherAdviceDay in teacherAdvicesDay)
                {
                    DateTime dateAdvice = teacherAdviceDay.DATE_ADVICE;
                    var hourAdvice = dateAdvice.Hour;

                    //se hace la comparacion por el valor del horario y la hora inicial del break
                    var itemToRemoveBreak = timesheethours.SingleOrDefault(x => x.ID_TIMESHEETHOURS == hourAdvice);
                    if (itemToRemoveBreak != null)
                        timesheethours.Remove(itemToRemoveBreak);

                }

                return timesheethours;
            }
            else {
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
