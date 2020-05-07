using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventSeries> EventSeries { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Rank> Ranks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Registration>()
                .Property(x => x.Status)
                .HasConversion<string>();


            base.OnModelCreating(builder);
        }
    }
}
