using System.ComponentModel.DataAnnotations;

namespace KURZ.Entities
{
    public class Timesheets
    {
        [Key]
        [Display(Name = "Id del Horario ")]
        public int ID_TIMESHEET { get; set; }

        [Required(ErrorMessage = "El horario es requerido")]
        public string? TIMESHEET { get; set; }

        [Required(ErrorMessage = "Para agregar un horario debe tener un profesor asociado")]
        public int ID_TEACHER { get; set; }
    }

    public class TimesheetHours
    { 
        public int ID_TIMESHEETHOURS { get; set; }

        public string? VALUE { get; set; }
        public string? NAME { get; set; }


        public TimesheetHours(int iD_TIMESHEETHOURS, string? vALUE, string? nAME)
        {
            ID_TIMESHEETHOURS = iD_TIMESHEETHOURS;
            VALUE = vALUE;
            NAME = nAME;
        }
    }

    public class TimesheetData {
        public string? day_off { get; set; }
        public string? start { get; set; }

        public string? end { get; set; }

        public dynamic? breaks { get; set; }
    }
}
