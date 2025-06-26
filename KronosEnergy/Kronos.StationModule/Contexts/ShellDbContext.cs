using Kronos.StationModule.Contexts.Common;
using Kronos.StationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kronos.StationModule.Contexts;

public class ShellDbContext : DbContext, BaseContext
{
    public ShellDbContext(DbContextOptions<ShellDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    public virtual DbSet<Station> Stations { get; set; }
    public virtual DbSet<Consept> Consepts { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<BusinessType> BusinessTypes { get; set; }
    public virtual DbSet<Machine> Machines { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }

}
