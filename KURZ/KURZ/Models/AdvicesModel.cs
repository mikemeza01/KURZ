using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ZoomNet.Models;

namespace KURZ.Models
{
    public class AdvicesModel : IAdvicesModel
    {
        private readonly KurzContext _context;

        private IHostEnvironment _hostingEnvironment;
        private readonly IUsersModel _usersModel;


        public AdvicesModel(KurzContext context,  IHostEnvironment hostingEnvironment, IUsersModel usersModel)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _usersModel = usersModel;
        }

        public string AdviceCreate(Advices advice, string host) {
            try {
                _context.Advices.Add(advice);
                _context.SaveChanges();

                var teacher = _usersModel.byID(advice.ID_TEACHER);
                
                string cuerpo = EmailTeacherAdvice(advice, host);

                try
                {
                    _usersModel.SendEmail(teacher.EMAIL, "Nueva Solicitud Asesoria", cuerpo);
                }
                catch (Exception ex)
                {
                    return "Asesoría creada pero hubo un error al enviar el correo de solicitud de asesoría al profesor, pongase en contacto con el administrador.";
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar la asesoría:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public Advices AdviceDetail(int? ID) {
            try
            {
                var advice = _context.Advices.Find(ID);
                return advice;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo Asesorías: " + ex.Message);
            }
        }

        public string AdvicesEdit(Advices advice)
        {
            try
            {
                _context.Advices.Update(advice);
                _context.SaveChanges();

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al editar la asesoría.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string notifyStudent(Advices advice, bool confirm, string host) {
            try
            {

                var student = _usersModel.byID(advice.ID_STUDENT);

                if (confirm)
                {
                    try
                    {
                        string cuerpo = EmailTeacherAdviceConfirm(advice, host);
                        _usersModel.SendEmail(student.EMAIL, "Solicitud Asesoría Confirmada", cuerpo);
                    }
                    catch (Exception ex)
                    {
                        return "Asesoría confirmada pero hubo un error al enviar el correo de solicitud de asesoría al estudiante, pongase en contacto con el administrador.";
                    }
                }
                else {
                    try
                    {
                        string cuerpo = EmailTeacherAdviceDecline(advice, host);
                        _usersModel.SendEmail(student.EMAIL, "Solicitud Asesoría Rechazada", cuerpo);
                    }
                    catch (Exception ex)
                    {
                        return "Asesoría confirmada pero hubo un error al enviar el correo de solicitud de asesoría al estudiante, pongase en contacto con el administrador.";
                    }
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar la asesoría:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }


        public string notifyStudentLink(int idAdvice, string host)
        {
            try
            {
                var adviceDetail = GetAdvicesById(idAdvice);
                var student = _usersModel.byID(adviceDetail.IDSTUDENT);
                    try
                    {
                        string cuerpo = EmailStudentLink(adviceDetail, host);
                        _usersModel.SendEmail(student.EMAIL, "Pago Asesoría Confirmado", cuerpo);
                        return "ok";
                    }
                    catch (Exception ex)
                    {
                        return "Asesoría confirmada pero hubo un error al enviar el correo de pago de asesoría confirmado al estudiante, pongase en contacto con el administrador.";
                    }

               
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar la asesoría:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public string notifyTeacherLink(int idAdvice, string host)
        {
            try
            {
                var adviceDetail = GetAdvicesById(idAdvice);
                var teacher = _usersModel.byID(adviceDetail.IDTEACHER);
                try
                {
                    string cuerpo = EmailTeacherLink(adviceDetail, host);
                    _usersModel.SendEmail(teacher.EMAIL, "Asesoría Confirmada", cuerpo);
                    return "ok";
                }
                catch (Exception ex)
                {
                    return "Asesoría confirmada pero hubo un error al enviar el correo con en enlace de la sesión al profesor, pongase en contacto con el administrador.";
                }


            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error al agregar la asesoría:");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }


        public List<GetAdvices_Result> GetAdvices()
        {
            try
            {
                string sql = "[dbo].[GetAdvices]";
                var results = _context.GetAdvices_Result.FromSqlRaw(sql).ToList();

                if (results.Count > 0)
                {
                    return results;
                }

                return new List<GetAdvices_Result>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de advices: " + ex.Message);
            }
        }

        public GetAdvicesById_Result GetAdvicesById(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesById] @ID_ADVICE";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@ID_ADVICE",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };

                var result = _context.GetAdvicesById_Result.FromSqlRaw(sql, param).ToList();

                if (result.Count > 0)
                {
                    return result.FirstOrDefault();
                }

                return new GetAdvicesById_Result();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el advice seleccionado: " + ex.Message);
            }
        }


        public List<GetAdvicesByTeacherId_Result> GetAdvicesByTeacherId(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesByTeacherId] @IdTeacher";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@IdTeacher",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };

                var result = _context.GetAdvicesByTeacherId_Result.FromSqlRaw(sql, param).ToList();

                if (result.Count > 0)
                {
                    return result;
                }

                return new List<GetAdvicesByTeacherId_Result>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de advices: " + ex.Message);
            }
        }

        public List<GetAdvicesByTeacherId_Result> GetAdvicesByStudentId(int id)
        {
            try
            {
                string sql = "[dbo].[GetAdvicesByStudentId] @IdStudent";

                var param = new SqlParameter[]
                {
                    new SqlParameter()
                    {
                        ParameterName= "@IdStudent",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = id
                    }
                };

                var result = _context.GetAdvicesByTeacherId_Result.FromSqlRaw(sql, param).ToList();

                if (result.Count > 0)
                {
                    return result;
                }

                return new List<GetAdvicesByTeacherId_Result>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de advices: " + ex.Message);
            }
        }

        public TeacherTopicsView TopicTeacherDetails(int ID_TEACHER, int ID_TOPIC)
        {
            try
            {
                var consulta = from pricT in _context.Price_Topics
                               join prof in _context.Users on pricT.ID_TEACHER equals ID_TEACHER
                               join cou in _context.Countries on prof.ID_COUNTRY equals cou.ID_COUNTRY
                               join t in _context.Topics on pricT.ID_TOPIC equals t.ID_TOPIC
                               join gra in _context.Grades on prof.ID_USER equals gra.ID_TEACHER into grade
                               from gra in grade.DefaultIfEmpty()
                               where t.ID_TOPIC == ID_TOPIC && prof.ID_USER == ID_TEACHER
                               select new TeacherTopicsView
                               {
                                   ID_TOPIC = t.ID_TOPIC,
                                   ID_TEACHER = prof.ID_USER,
                                   TeacherName = prof.NAME,
                                   TeacherCountry = cou.NAME,
                                   Price = pricT.PRICE,
                                   NAME = t.NAME,
                                   AvgGrade = gra != null ? gra.AverageRating.GetValueOrDefault(5) : 5
                               };

                var TopicTeacherDetail = consulta.First();

                return TopicTeacherDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en obteniendo los datos del profesor y tema para solicitar asesoría: " + ex.Message);
            }
        }

        public List<Advices> AdvicesforTeacherAndDate(int ID_TEACHER, DateTime DATE_SELECTED) {

            try
            {
                var consulta = from adv in _context.Advices
                               where adv.ID_TEACHER == ID_TEACHER && 
                               (adv.DATE_ADVICE.Date >= DATE_SELECTED)
                               select new Advices
                               { 
                                   DATE_ADVICE = adv.DATE_ADVICE
                               };

                return consulta.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }

        public string EmailTeacherAdvice(Advices advice, String host)
        {
            var adviceResult = GetAdvicesById(advice.ID_ADVICE);


            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//ConfirmarAsesoriaTeacher.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@NombreProfesor", adviceResult.TEACHERNAME);
            htmlArchivo = htmlArchivo.Replace("@@DATE_ADVICE", adviceResult.DATE_ADVICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@PRICE", adviceResult.PRICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@STUDENT", adviceResult.STUDENTNAME); 
            htmlArchivo = htmlArchivo.Replace("@@TOPIC", adviceResult.TOPICNAME);

            htmlArchivo = htmlArchivo.Replace("@@CONFIRMADVICE", host + "/Advices/ConfirmAdvice/?advice=" + adviceResult.ID_ADVICE);
            htmlArchivo = htmlArchivo.Replace("@@DECLINEADVICE", host + "/Advices/DeclineAdvice/?advice=" + adviceResult.ID_ADVICE);

            return htmlArchivo;
        }

        public string EmailTeacherAdviceConfirm(Advices advice, String host)
        {
            var adviceResult = GetAdvicesById(advice.ID_ADVICE);


            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//NotificacionAsesoriaAceptada.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@NombreProfesor", adviceResult.TEACHERNAME);
            htmlArchivo = htmlArchivo.Replace("@@DATE_ADVICE", adviceResult.DATE_ADVICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@PRICE", adviceResult.PRICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@STUDENT", adviceResult.STUDENTNAME);
            htmlArchivo = htmlArchivo.Replace("@@TOPIC", adviceResult.TOPICNAME);

            htmlArchivo = htmlArchivo.Replace("@@CONFIRMADVICE", host + "/Advices/ConfirmAdviceStudent/?advice=" + adviceResult.ID_ADVICE);

            return htmlArchivo;
        }

        public string EmailTeacherAdviceDecline(Advices advice, String host)
        {
            var adviceResult = GetAdvicesById(advice.ID_ADVICE);


            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//NotificacionAsesoriaRechazada.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@NombreProfesor", adviceResult.TEACHERNAME);
            htmlArchivo = htmlArchivo.Replace("@@DATE_ADVICE", adviceResult.DATE_ADVICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@PRICE", adviceResult.PRICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@STUDENT", adviceResult.STUDENTNAME);
            htmlArchivo = htmlArchivo.Replace("@@TOPIC", adviceResult.TOPICNAME);

            return htmlArchivo;
        }

        public string EmailStudentLink(GetAdvices_Result adviceResult, String host)
        {


            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//NotificacionAsesoriaLinkStudent.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@NombreProfesor", adviceResult.TEACHERNAME);
            htmlArchivo = htmlArchivo.Replace("@@DATE_ADVICE", adviceResult.DATE_ADVICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@PRICE", adviceResult.PRICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@STUDENT", adviceResult.STUDENTNAME);
            htmlArchivo = htmlArchivo.Replace("@@TOPIC", adviceResult.TOPICNAME);
            htmlArchivo = htmlArchivo.Replace("@@LINK", adviceResult.LINK);

            return htmlArchivo;
        }

        public string EmailTeacherLink(GetAdvices_Result adviceResult, String host)
        {


            string rutaArchivo = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/CorreoTemplate//NotificacionAsesoriaLinkTeacher.html");
            string htmlArchivo = System.IO.File.ReadAllText(rutaArchivo);
            htmlArchivo = htmlArchivo.Replace("@@NombreProfesor", adviceResult.TEACHERNAME);
            htmlArchivo = htmlArchivo.Replace("@@DATE_ADVICE", adviceResult.DATE_ADVICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@PRICE", adviceResult.PRICE.ToString());
            htmlArchivo = htmlArchivo.Replace("@@STUDENT", adviceResult.STUDENTNAME);
            htmlArchivo = htmlArchivo.Replace("@@TOPIC", adviceResult.TOPICNAME);
            htmlArchivo = htmlArchivo.Replace("@@LINK", adviceResult.LINK);

            return htmlArchivo;
        }

    }
}
