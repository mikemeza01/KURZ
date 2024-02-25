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
                                StudentName =  s.NAME +" "+ s.LASTNAME,
                                TeacherName= t.NAME +" "+ t.LASTNAME,
                                Topic = at.NAME,
                                Category = c.NAME,
                                Subcategory = sc.NAME


                            };
                return query.ToList();  
            }
            catch (Exception)
            {

                throw;
            }

        }//TeacherGradesByUser

    }//Public Class

}//Kurz.Models

