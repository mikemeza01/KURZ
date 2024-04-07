using KURZ.Interfaces;
using KURZ.Entities;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class TimesheetsModel : ITimesheetsModel
    {
        //se llama el contexto de la base de datos
        private readonly KurzContext _context;

        //constructor de la clase y recibe como parametro el contexto de la base de datos
        public TimesheetsModel(KurzContext context)
        {
            _context = context;
        }

        public Timesheets TimesheetDetailbyTeacher(int? ID_TEACHER)
        {
            try
            {

                var timesheet = _context.Timesheets.Where(d => d.ID_TEACHER == ID_TEACHER).FirstOrDefault();

                return timesheet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de categorías: " + ex.Message);
            }
        }


        public Timesheets TimesheetDetail(int? ID)
        {
            try
            {
                var timesheet = _context.Timesheets.Find(ID);
                return timesheet;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno en el modelo de Horarios: " + ex.Message);
            }
        }

        public string Update(Timesheets timesheets)
        {
            try
            {
                var existTimesheet = TimesheetDetailbyTeacher(timesheets.ID_TEACHER);

                if (existTimesheet != null)
                {
                    existTimesheet.TIMESHEET = timesheets.TIMESHEET;
                    _context.Timesheets.Update(existTimesheet);
                    _context.SaveChanges();
                }
                else {
                    _context.Timesheets.Add(timesheets);
                    _context.SaveChanges();
                }

                return "ok";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Ocurrió un error crear o actualizar el horario.");
                Console.WriteLine(ex.ToString());
                return "error";
            }
        }

        public List<TimesheetHours> TimesheetHours()
        {
            var timesheethours = new List<TimesheetHours>();
            timesheethours.Add(new TimesheetHours(0, "00:00", "12:00 a.m."));
            timesheethours.Add(new TimesheetHours(1, "01:00", "01:00 a.m."));
            timesheethours.Add(new TimesheetHours(2, "02:00", "03:00 a.m."));
            timesheethours.Add(new TimesheetHours(3, "03:00", "03:00 a.m."));
            timesheethours.Add(new TimesheetHours(4, "04:00", "04:00 a.m."));
            timesheethours.Add(new TimesheetHours(5, "05:00", "05:00 a.m."));
            timesheethours.Add(new TimesheetHours(6, "06:00", "06:00 a.m."));
            timesheethours.Add(new TimesheetHours(7, "07:00", "07:00 a.m."));
            timesheethours.Add(new TimesheetHours(8, "08:00", "08:00 a.m."));
            timesheethours.Add(new TimesheetHours(9, "09:00", "09:00 a.m."));
            timesheethours.Add(new TimesheetHours(10, "10:00", "10:00 a.m."));
            timesheethours.Add(new TimesheetHours(11, "11:00", "11:00 a.m."));
            timesheethours.Add(new TimesheetHours(12, "12:00", "12:00 p.m."));
            timesheethours.Add(new TimesheetHours(13, "13:00", "01:00 p.m."));
            timesheethours.Add(new TimesheetHours(14, "14:00", "02:00 p.m."));
            timesheethours.Add(new TimesheetHours(15, "15:00", "03:00 p.m."));
            timesheethours.Add(new TimesheetHours(16, "16:00", "04:00 p.m."));
            timesheethours.Add(new TimesheetHours(17, "17:00", "05:00 p.m."));
            timesheethours.Add(new TimesheetHours(18, "18:00", "06:00 p.m."));
            timesheethours.Add(new TimesheetHours(19, "19:00", "07:00 p.m."));
            timesheethours.Add(new TimesheetHours(20, "20:00", "08:00 p.m."));
            timesheethours.Add(new TimesheetHours(21, "21:00", "09:00 p.m."));
            timesheethours.Add(new TimesheetHours(22, "22:00", "10:00 p.m."));
            timesheethours.Add(new TimesheetHours(23, "23:00", "11:00 p.m."));;

            return timesheethours;
        }
    }
}
