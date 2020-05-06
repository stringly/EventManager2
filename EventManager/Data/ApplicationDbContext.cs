using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
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
