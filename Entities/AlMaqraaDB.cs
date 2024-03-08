using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AlMaqraaDB : IdentityDbContext<User>
{
    //EntityFrameworkCore\Add-Migration "name"
    //EntityFrameworkCore\Update-Database
    public AlMaqraaDB() { }
    public AlMaqraaDB(DbContextOptions<AlMaqraaDB> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID =postgres;Password=1234;Server=localhost;Port=5432;Database=gamal; Pooling = true; ");
        //optionsBuilder.UseNpgsql("User ID =postgres;Password=123456;Server=localhost;Port=5432;Database=CareerCareHub;Integrated Security = true; Pooling = true; ");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Statistics>()
      .HasOne(s => s.User)
      .WithOne(u => u.Statistics)
      .HasForeignKey<Statistics>(s => s.UserId)
      .HasPrincipalKey<User>(u => u.Id);
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
    public virtual DbSet<Ayah> Ayat { get; set; }
    public virtual DbSet<Surah> Surahs { get; set; }
    public virtual DbSet<Sheikh> Sheikhs { get; set; }
    public virtual DbSet<Statistics> Statistics { get; set; }
}
