using KURZ.Entities;
using KURZ.Helpers;
using KURZ.Interfaces;
using KURZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using ZoomNet;
using ZoomNet.Models;
using ZoomNet.Resources;

namespace KURZ.Controllers
{
    
    public class AdvicesController : Controller
    {
        private readonly IAdvicesModel _advicesModel;
        //llamado a los modelos a usar
        private readonly IUsersModel _usersModel;
        private readonly IConfiguration _configuration;


        private const string AccountId = "OAg6Ia1DS1K0bjF_KVgmYQ";
        private const string ZoomApiKey = "rfTbKS3zQJOCjw7wYUlCw";
        private const string ZoomApiSecret = "kTwuSsWKo448kJmfVaj3lHqT6YsGCRpY";
        private const string ApiURL = "https://api.zoom.us/v2";
        private const string ApiUser = "info@educandoteya.org";
        private FilesHelper filesHelper = new FilesHelper();

        public AdvicesController(IAdvicesModel advicesModel, IUsersModel usersModel, IConfiguration configuration)
        {
            _advicesModel = advicesModel;
            _usersModel = usersModel;
            _configuration = configuration;
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult CreateAdvice(int te, int to) 
        {
            //te = teacher
            //to = topic
            
            var TeacherTopicDetail = _advicesModel.TopicTeacherDetails(te, to);

            ViewBag.teacherTopicDetail = TeacherTopicDetail;
            var user = _usersModel.byID(te);

            var userDetail = _usersModel.ConvertUsers(user);

            userDetail.ProfilePicture = filesHelper.ReadFiles(userDetail.PHOTO ?? "", _configuration.GetSection("Variables:carpetaFotos").Value + "\\" + userDetail.IDENTICATION);
            ViewBag.user_photo = userDetail.ProfilePicture;


            ClaimsPrincipal claimstudent = HttpContext.User;
            string nombreusuario = "";

            if (claimstudent.Identity.IsAuthenticated)
            {
                nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            var student = _usersModel.byUserName(nombreusuario);

            ViewBag.ID_STUDENT = student.ID_USER;

            return View();
        }

        [HttpPost]
        public string CreateAdvice(string dateAdvice, Double price, int idTeacher, int idTopic, int idStudent)
        {

            try
            {
                //domain to send email
                var request = HttpContext.Request;
                var host = request.Host.ToUriComponent();
                var pathBase = request.PathBase.ToUriComponent();
                var domain = $"{request.Scheme}://{host}{pathBase}";


                DateTime DATE_ADVICE = DateTime.Parse(dateAdvice);
                DateTime CURRENT_TIME = DateTime.Now;

                var Advice = new Advices()
                {
                    DATE_ADVICE = DATE_ADVICE,
                    DATE_CREATE = CURRENT_TIME,
                    DATE_UPDATE = CURRENT_TIME,
                    PRICE = price,
                    LINK = "",
                    ID_TOPIC = idTopic,
                    ID_TEACHER = idTeacher,
                    ID_STUDENT = idStudent,
                    ID_STATUS = 1, //1 es el ID del status en espera.
                };

                var resultado = _advicesModel.AdviceCreate(Advice, domain);

                if (resultado == "ok")
                {
                    return "ok";
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception)
            {
                return "error";
            }                
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult GetAdvices()
        {
            try
            {
                var advices = _advicesModel.GetAdvices();
                return View(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult AdvicesStudent()
        {
            try
            {
                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var student = _usersModel.byUserName(nombreusuario);

                var advices = _advicesModel.GetAdvicesByStudentId(student.ID_USER);

                return View(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult AdvicesTeacher()
        {
            try
            {
                ClaimsPrincipal claimstudent = HttpContext.User;
                string nombreusuario = "";

                if (claimstudent.Identity.IsAuthenticated)
                {
                    nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                }
                var teacher = _usersModel.byUserName(nombreusuario);

                var advices = _advicesModel.GetAdvicesByTeacherId(teacher.ID_USER);

                return View(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult ConfirmAdvice(string advice)
        {
            try
            {
                if (advice != "")
                {
                    //domain to send email
                    var request = HttpContext.Request;
                    var host = request.Host.ToUriComponent();
                    var pathBase = request.PathBase.ToUriComponent();
                    var domain = $"{request.Scheme}://{host}{pathBase}";

                    ClaimsPrincipal claimstudent = HttpContext.User;
                    string nombreusuario = "";

                    if (claimstudent.Identity.IsAuthenticated)
                    {
                        nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    }
                    var teacher = _usersModel.byUserName(nombreusuario);


                    var adviceDetail = _advicesModel.AdviceDetail(Int32.Parse(advice));


                    if (adviceDetail.ID_TEACHER == teacher.ID_USER)
                    {
                        if (adviceDetail.ID_STATUS != 2)
                        {
                            adviceDetail.ID_STATUS = 2; //Solicitud Aceptada

                            var resultado = _advicesModel.AdvicesEdit(adviceDetail);

                            var notifyStudent = _advicesModel.notifyStudent(adviceDetail, true, domain);

                            ViewBag.result = "ok";

                            return View(adviceDetail);
                        }
                        else {
                            ViewBag.result = "already_confirm";

                            return View(adviceDetail);
                        }
                    }
                    else
                    {
                        ViewBag.result = "error_teacher";

                        return View(adviceDetail);
                    }
                }
                else {
                    ViewBag.result = "error_id";

                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.result = "error";

                return View();
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult DeclineAdvice(string advice)
        {
            try
            {
                if (advice != "")
                {
                    //domain to send email
                    var request = HttpContext.Request;
                    var host = request.Host.ToUriComponent();
                    var pathBase = request.PathBase.ToUriComponent();
                    var domain = $"{request.Scheme}://{host}{pathBase}";

                    ClaimsPrincipal claimstudent = HttpContext.User;
                    string nombreusuario = "";

                    if (claimstudent.Identity.IsAuthenticated)
                    {
                        nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    }
                    var teacher = _usersModel.byUserName(nombreusuario);


                    var adviceDetail = _advicesModel.AdviceDetail(Int32.Parse(advice));


                    if (adviceDetail.ID_TEACHER == teacher.ID_USER)
                    {
                        if (adviceDetail.ID_STATUS != 3)
                        {
                            adviceDetail.ID_STATUS = 3; //Solicitud Rechazada

                            var resultado = _advicesModel.AdvicesEdit(adviceDetail);

                            var notifyStudent = _advicesModel.notifyStudent(adviceDetail, true, domain);

                            ViewBag.result = "ok";

                            return View(adviceDetail);
                        }
                        else {
                            ViewBag.result = "already_decline";

                            return View(adviceDetail);
                        }
                    }
                    else
                    {
                        ViewBag.result = "error_teacher";

                        return View(adviceDetail);
                    }
                }
                else
                {
                    ViewBag.result = "error_id";

                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.result = "error";

                return View();
            }
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult ConfirmAdviceStudent(string advice)
        {
            try
            {
                if (advice != "")
                {

                    ClaimsPrincipal claimstudent = HttpContext.User;
                    string nombreusuario = "";

                    if (claimstudent.Identity.IsAuthenticated)
                    {
                        nombreusuario = claimstudent.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    }
                    var student = _usersModel.byUserName(nombreusuario);


                    var adviceDetail = _advicesModel.AdviceDetail(Int32.Parse(advice));


                    if (adviceDetail.ID_STUDENT == student.ID_USER)
                    {
                        if (adviceDetail.ID_STATUS != 5)
                        {
                            adviceDetail.ID_STATUS = 5; //Solicitud Pendiente de Pago, el estudiante confirma que realizo el pago, el administrador lo verifica

                            var resultado = _advicesModel.AdvicesEdit(adviceDetail);

                            ViewBag.result = "ok";

                            return View(adviceDetail);
                        }
                        else {
                            ViewBag.result = "already_confirm";

                            return View(adviceDetail);
                        }
                    }
                    else
                    {
                        ViewBag.result = "error_student";

                        return View(adviceDetail);
                    }
                }
                else
                {
                    ViewBag.result = "error_id";

                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.result = "error";

                return View();
            }
        }

        [Authorize(Roles = "Student,Teacher,Administrator")]
        [HttpGet]
        public IActionResult DetailsAdviceId(int id)
        {
            try
            {
                var advice = _advicesModel.GetAdvicesById(id);
                if (advice == null)
                {
                    ViewData["Error"] = 1;
                    return View();
                }
                return View(advice);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult DetailsAdvice(int id)
        {
            try
            {
                var advices = _advicesModel.GetAdvicesById(id);

                return Json(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult CreateMeeting()
        {
            var meeting = new MeetingsZoom();
            return View(meeting);
        }


        [HttpPost]
        public async Task<ActionResult> CreateMeeting(MeetingsZoom MeetingZoom)
        {
            string meetingTopic = "Título de la reunión";
            DateTime meetingStartTime = DateTime.UtcNow.AddMinutes(10);
            int meetingDuration = 60; //Duración de la sesión en minutos
            //TimeZones timeZonesDetail = new TimeZones();


            var zoomClient = getZoomClient();

            var meeting = await zoomClient.Meetings.CreateScheduledMeetingAsync(ApiUser, meetingTopic, meetingTopic, meetingStartTime, meetingDuration, TimeZones.America_Costa_Rica);


            MeetingZoom.ID = meeting.Id;
            MeetingZoom.DURATION = meeting.Duration;
            MeetingZoom.START_TIME = meeting.StartTime;
            MeetingZoom.JOIN_URL = meeting.JoinUrl;

            //var accessToken = await generateToken();

            /*HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            

            if (string.IsNullOrEmpty(accessToken))
            {
                // Handle error when unable to retrieve the access token
                return View();
            }


            string createMeetingEndpoint = ApiURL + "/users/" + ApiUser + "/meetings";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestBody = new
            {
                topic = meetingTopic,
                type = 2,
                start_time = meetingStartTime,
                duration = meetingDuration,
                timeZone = "America/Costa_Rica",
                settings = new
                {
                    host_video = true,
                    participant_video = true,
                    allow_multiple_device = true,
                }
            };

            var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(createMeetingEndpoint, requestContent);
            var responseContent = response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Se creo correctamenta la sesión";
                ViewBag.Response = responseContent;
            }
            else
            {
                ViewBag.Message = "Fallo al crear la sesión";
                ViewBag.Response = responseContent;
            }

            */

            return View(MeetingZoom);
        }

        private ZoomClient getZoomClient()
        {

            var clientId = ZoomApiKey;
            var clientSecret = ZoomApiSecret;
            var accountId = AccountId;
            var refreshToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", EnvironmentVariableTarget.User);
            var connectionInfo =  OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId,
            (_, newAccessToken) =>
            {
                /*
                    Server-to-Server OAuth does not use a refresh token. That's why I used '_' as the first parameter
                    in this delegate declaration. Furthermore, ZoomNet will take care of getting a new access token
                    and to refresh it whenever it expires therefore there is no need for you to preserve it.

                    In fact, this delegate is completely optional when using Server-to-Server OAuth. Feel free to pass
                    a null value in lieu of a delegate.
                */
            });

            var zoomClient = new ZoomClient(connectionInfo);
            /*var httpClient = new HttpClient();

            var request = new
            {
                client_id = ZoomApiKey,
                response_type = "code",
                redirect_uri = "https://localhost:7060/Advices/CreateMeeting/GetCode"
            };

            string tokenUrl = "https://zoom.us/oauth/authorize";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(tokenUrl, content);


            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                return result.access_token;
            }*/

            return zoomClient;
        
        }

        private async Task<string> generateToken() {
            var httpClient = new HttpClient();

            var request = new
            {
                grant_type = "client_credentials",
                client_id = ZoomApiKey,
                client_secret = ZoomApiSecret
            };

            string tokenUrl = "https://zoom.us/oauth/token";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            var content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.PostAsync(tokenUrl, content);


            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                return result.access_token;
            }

            return null;
        }


        [HttpGet]
        public IActionResult GetCode(string code)
        {
            
            return View();
        }
    }
}
