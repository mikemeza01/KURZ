using KURZ.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;

namespace KURZ.Models
{
    public class KurzContext : DbContext
    {
        //Constructor del contexto
        public KurzContext(DbContextOptions<KurzContext> options)
        : base(options)
        {
        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<Topicss> Topicss { get; set; }
        public DbSet<Advices> Advices { get; set; }
        public DbSet<GetAdvices_Result> GetAdvices_Result { get; set; }
        public DbSet<GetAdvicesById_Result> GetAdvicesById_Result { get; set; }
        public DbSet<GetAdvicesByTeacherId_Result> GetAdvicesByTeacherId_Result { get; set; }
    }
}
