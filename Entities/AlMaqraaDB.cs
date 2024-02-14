using Al_Mqraa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerCareHub.Entity

{
    public class AlMaqraaDB: IdentityDbContext<User>   
    {
        //EntityFrameworkCore\Add-Migration "name"
        //EntityFrameworkCore\Update-Database
        public AlMaqraaDB() { }
        public AlMaqraaDB(DbContextOptions<AlMaqraaDB> options): base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID =postgres;Password=F55BaG1cEg41eF1B2c1Ec5e4c2e5CcBe;Server=roundhouse.proxy.rlwy.net;Port=21848;Database=railway;Integrated Security = true; Pooling = true; ");
            //optionsBuilder.UseNpgsql("User ID =postgres;Password=123456;Server=localhost;Port=5432;Database=CareerCareHub;Integrated Security = true; Pooling = true; ");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Statistics)
                .WithOne(s => s.User)
                .HasForeignKey<Statistics>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust the delete behavior as needed
            //SeedRoles(modelBuilder);
        }
/*        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "ADMIN",ConcurrencyStamp="1",NormalizedName = "ADMIN" }
                );
            
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "SUBSCRIBER",ConcurrencyStamp="2",NormalizedName = "SUBSCRIBER" }
                );
            builder.Entity<IdentityRole>().HasData(
               new IdentityRole() { Name = "USER", ConcurrencyStamp = "3", NormalizedName = "USER" }
               );

        }*/
        public virtual DbSet<User> Users { get; set; }


    }
}
