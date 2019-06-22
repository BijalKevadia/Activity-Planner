using Microsoft.EntityFrameworkCore;

namespace activityPlanner.Models {
    public class Context : DbContext {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActivityMod> Activities  { get; set; }
        public DbSet<Rsvp> Rsvps { get; set; }
    }
}