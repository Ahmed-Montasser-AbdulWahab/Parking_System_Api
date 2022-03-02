using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.Entities;

namespace Parking_System_API.Data.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ): base(options)
        {

        }


        public DbSet<SystemUser> SystemUsers { get; set; }


        //protected override void OnModelCreating(ModelBuilder mb)
        //{
        //   mb.Entity<SystemUser>().HasKey(p => new { p.Email });

        //    mb.Entity<SystemUser>()
        //.HasData(new
        //{
        //    Email = "admin@admin.com",
        //    Name = "admin",
        //    Password = "admin",
        //    Type = true
        //});

        //    mb.Entity<SystemUser>()
        //      .HasData(new
        //      {
        //          Email = "operator@operator.com",
        //          Name = "operator",
        //          Password = "operator",
        //          Type = false
        //      });

        //}
    }
}
