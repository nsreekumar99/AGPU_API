using AGPU_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AGPU_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<AGPU> AGPUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AGPU>().HasData(
                new AGPU()
                {
                    Id = 1,
                    Name = "Nvidia RTX 3060" ,
                    Brand = "Nvidia",
                    Model = "RTX 3060",
                    ReleaseDate = new DateTime(2021,2,25).Date,
                    Price = 28000,
                    ValuePercentage = 94.8,
                    AverageBenchPercentage = 100,
                    Images = new List<string> { "/Uploads/GPU/RTX_3060.jpg"}
                },
                new AGPU()
                {
                    Id = 2,
                    Name = "Nvidia GTX 1660S (Super)",
                    Brand = "Nvidia",
                    Model = "GTX 1660S (Super)",
                    ReleaseDate = new DateTime(2019,10,29).Date,
                    Price = 20000,
                    ValuePercentage = 70.8,
                    AverageBenchPercentage = 71.4,
                    Images = new List<string> { "/Uploads/GPU/GTX_1660SUPER.jpg" }
                });
        }
    }
}
