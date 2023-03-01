using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace web_project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Auction> Auction { get; set; }
        public DbSet<Models.User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        // Category enum
        public enum Category
        {
            Electronics = 1,
            Home = 2,
            Fashion = 3,
            Sports = 4,
            Other = 5
        }

        // Condition enum
        public enum Condition
        {
            New = 1,
            Used = 2
        }
    }
}