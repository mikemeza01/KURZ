using KURZ.Entities;

namespace KURZ.Interfaces
{
    public interface ITimesheetsModel
    {
        public Timesheets TimesheetDetailbyTeacher(int? ID_TEACHER);

        public Timesheets TimesheetDetail(int? ID);

        public string Update(Timesheets timesheets);

        public List<TimesheetHours> TimesheetHours();
    }
}
