using Kronos.StationModule.Contexts.Common;
using Kronos.StationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kronos.StationModule.Contexts
{
    public class AutomationDbContext : DbContext, BaseContext
    {
        public AutomationDbContext(DbContextOptions<AutomationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Consept> Consepts { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
