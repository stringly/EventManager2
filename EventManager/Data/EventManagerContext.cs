using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventSeries> EventSerieses { get; set; }        
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<EventModule> EventModules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Rank configuration
            var navigation5 = builder.Entity<Rank>()
                .Metadata.FindNavigation(nameof(Rank.Users));
            navigation5.SetPropertyAccessMode(PropertyAccessMode.Field);
            #endregion
            #region Event configuration
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
            builder.Entity<Event>()
                .HasOne(typeof(User), "Owner").WithMany("_ownedEvents");            

            // Set Event.Registrations collection shadow property
            var navigation = builder.Entity<Event>()
                .Metadata.FindNavigation(nameof(Event.Registrations));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Set Event.Attendance collection shadow property
            var navigation6 = builder.Entity<Event>()
                .Metadata.FindNavigation(nameof(Event.Attendance));
            navigation6.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Set Event.Modules collection shadow property
            var navigation7 = builder.Entity<Event>()
                .Metadata.FindNavigation(nameof(Event.EventModules));
            navigation7.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.Entity<Event>().Property<DateTime>("Created");
            builder.Entity<Event>().Property<DateTime>("LastModified");
            #endregion
            #region User configuration
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
            // Set User.Registrations collection shadow property
            var navigation1 = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.Registrations));
            navigation1.SetPropertyAccessMode(PropertyAccessMode.Field);
            // Set User.OwnedEvents collection shadow property
            var navigation2 = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.OwnedEvents));
            navigation2.SetPropertyAccessMode(PropertyAccessMode.Field);
            // Set Event.Attendance collection shadow property
            var navigation9 = builder.Entity<User>()
                .Metadata.FindNavigation(nameof(User.Attendance));
            navigation9.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Entity<User>().Property<DateTime>("Created");
            builder.Entity<User>().Property<DateTime>("LastModified");
            #endregion
            #region Registration configuration
            builder.Entity<Registration>()
                .Property(x => x.Status)
                .HasConversion<string>();
            #endregion
            #region EventSeries configuration
            var navigation3 = builder.Entity<EventSeries>()
                .Metadata.FindNavigation(nameof(EventSeries.Events));
            navigation3.SetPropertyAccessMode(PropertyAccessMode.Field);
            #endregion
            #region EventType configuration
            var navigation4 = builder.Entity<EventType>()
                .Metadata.FindNavigation(nameof(EventType.Events));
            navigation4.SetPropertyAccessMode(PropertyAccessMode.Field);
            #endregion
            #region EventModule configuration

            // Set EventModule.Attendance collection shadow property
            var navigation8 = builder.Entity<EventModule>()
                .Metadata.FindNavigation(nameof(EventModule.Attendance));
            navigation8.SetPropertyAccessMode(PropertyAccessMode.Field);
            #endregion
            #region Attendance configuration

            builder.Entity<Attendance>()
                .Property(x => x.Status)
                .HasConversion<string>();
            #endregion
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            var timestamp = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                    .Where(e => (e.Entity is Event || e.Entity is User) &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified)
                    ))
            {
                entry.Property("LastModified").CurrentValue = timestamp;
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = timestamp;
                }

            }
            return base.SaveChanges();
        }
    }
}
