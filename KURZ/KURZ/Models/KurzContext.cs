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
    }
}
