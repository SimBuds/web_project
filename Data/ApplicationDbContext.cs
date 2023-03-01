using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_project.Models;

namespace web_project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Models.Auction> Auction { get; set; }
        public DbSet<Models.User> User { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Auction entity
            modelBuilder.Entity<Auction>()
                .Property(a => a.Category)
                .HasConversion<int>();

            modelBuilder.Entity<Auction>()
                .Property(a => a.Condition)
                .HasConversion<int>();

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.User)
                .WithMany(u => u.Auctions)
                .HasForeignKey(a => a.UserId);

            // Configure the User entity
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();
        }
    }
}
