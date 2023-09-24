using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class KurzContext : DbContext
    {
        public KurzContext(DbContextOptions<KurzContext> options)
        : base(options)
        {

        }
    }
}
