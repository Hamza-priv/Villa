using Microsoft.EntityFrameworkCore;
using Villa.Model;

namespace Villa.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<VillaModel> Villas { get; set; }


    }
}
