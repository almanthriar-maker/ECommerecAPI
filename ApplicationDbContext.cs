using E_CommerceSystem_API.Controllers;
using E_CommerceSystem_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem_API
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Order> Orders { get; set; }

        public DbSet<Models.Review> Reviews { get; set; }

        public DbSet<Models.OrderProducts> OrderProducts { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ECommerceDB-API;Trusted_Connection=True");
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProducts>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
