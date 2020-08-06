using System;
using BikeToIt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeToIt.Data
{
    public class TrailDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<DestinationCategory> DestinationCategories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TrailCity> TrailCity { get; set; }

        public TrailDbContext(DbContextOptions<TrailDbContext> options) : base(options)
        {
        }

        public TrailDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrailCity>()
                .HasKey(tc => new { tc.TrailId, tc.CityId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
