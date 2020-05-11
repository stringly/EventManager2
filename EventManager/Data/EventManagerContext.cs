using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class EventManagerContext : DbContext
    {
        public EventManagerContext(DbContextOptions<EventManagerContext> options)
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
            // property magic string shadow property
            builder.Entity<Event>()
                .Property(x => x.Title)
                .HasField("_title");
            builder.Entity<Event>()
                .Property(x => x.Description)
                .HasField("_description");
            builder.Entity<Event>()
                .Property(x => x.FundCenter)
                .HasField("_fundCenter");
            builder.Entity<Event>().OwnsOne(p => p.AddressFactory);
            // Collection shadow property
            var navigation = builder.Entity<Event>()
                .Metadata.FindNavigation(nameof(Event.Registrations));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);


            builder.Entity<User>()
                .Property(x => x.LDAPName)
                .HasField("_LDAPName");
            builder.Entity<User>()
                .Property(x => x.BlueDeckId)
                .HasField("_blueDeckId");
            builder.Entity<User>()
                .Property(x => x.IdNumber)
                .HasField("_idNumber");
            builder.Entity<User>()
                .Property(x => x.Email)
                .HasField("_email");
            builder.Entity<User>()
                .Property(x => x.ContactNumber)
                .HasField("_contactNumber");
            builder.Entity<User>().OwnsOne(p => p.NameFactory);
            // Collection shadow property
            var navigation1 = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.Registrations));
            navigation1.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigation2 = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.OwnedEvents));
            navigation2.SetPropertyAccessMode(PropertyAccessMode.Field);






            builder.Entity<Registration>()
                .Property(x => x.Status)
                .HasConversion<string>();


            base.OnModelCreating(builder);
        }
    }
}
