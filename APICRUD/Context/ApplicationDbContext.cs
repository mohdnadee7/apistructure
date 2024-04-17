using APICRUD.Entities;
using APICRUD.Model;
using Microsoft.EntityFrameworkCore;

namespace APICRUD.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Register> Registers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Register>().ToTable("tbl_register");
        }
    }
}
