using Microsoft.EntityFrameworkCore;
using PlannerTwo.Models;

namespace PlannerTwo.Context
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users { get; set; }

        // public DbSet<LoginUser> LoginUsers { get; set; }

        public DbSet<Wedding> Weddings { get; set; }

        public DbSet<RSVP> RSVPs { get; set; }
    }
}