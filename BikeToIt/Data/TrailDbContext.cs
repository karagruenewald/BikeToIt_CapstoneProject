using System;
using BikeToIt.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeToIt.Data
{
    public class TrailDbContext: DbContext
    {
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<DestinationCategory> DestinationCategories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TrailCity> TrailCity { get; set; }

        public TrailDbContext(DbContextOptions<TrailDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrailCity>()
                .HasKey(tc => new { tc.TrailId, tc.CityId });
        }
    }
}
