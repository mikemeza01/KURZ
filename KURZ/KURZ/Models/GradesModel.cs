using KURZ.Interfaces;
using KURZ.Entities;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{

    public class GradesModel : IGradesModel
    {
        private readonly KurzContext _context;

        public GradesModel(KurzContext context)
        {
            _context = context;
        }

        public List<TeacherGradesView> TeacherGradesByUser(int teacherId)
        {
            try
            {
                var query = from g in _context.Grades
                            join a in _context.Advices on g.ID_ADVICE equals a.ID_ADVICE
                            join t in _context.Users on g.ID_TEACHER equals t.ID_USER
                            join s in _context.Users on g.ID_STUDENT equals s.ID_USER
                            join at in _context.Topics on a.ID_TOPIC equals at.ID_TOPIC
                            join c in _context.Categories on at.ID_CATEGORY equals c.ID_CATEGORY
                            join sc in _context.SubCategories on at.ID_SUBCATEGORY equals sc.ID_SUBCATEGORY
                            where g.ID_TEACHER == teacherId

                            select new TeacherGradesView
                            {
                                ID_GRADE = g.ID_GRADE,
                                COMMENTARY = g.COMMENTARY,
                                GRADE = g.GRADE,
                                StudentName = s.NAME + " " + s.LASTNAME,
                                TeacherName = t.NAME + " " + t.LASTNAME,
                                Topic = at.NAME,
                                Category = c.NAME,
                                Subcategory = sc.NAME,
                                AverageRating = g.AverageRating

                            };
                return query.ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }//TeacherGradesByUser

        public object TeacherinfoByID(int adviceID)
        {
            try
            {
                var query = from a in _context.Advices
                            join g in _context.Grades on a.ID_ADVICE equals g.ID_ADVICE
                            join t in _context.Topics on a.ID_TOPIC equals t.ID_TOPIC
                            join c in _context.Categories on t.ID_CATEGORY equals c.ID_CATEGORY
                            join u in _context.Users on a.ID_TEACHER equals u.ID_USER
                            join s in _context.Status on a.ID_STATUS equals s.ID_STATUS
                            where a.ID_ADVICE == adviceID

                            select new TeacherGradesView
                            {
                                ID_GRADE = g.ID_GRADE,
                                TeacherName = u.NAME + " " + u.LASTNAME,
                                Topic = t.NAME,
                                Category = c.NAME
                            };


                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw ex;
            }
        }


    }//Public Class

}//Kurz.Models

