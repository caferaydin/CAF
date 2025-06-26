using Kronos.StationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kronos.StationModule.Contexts.Common;

public interface BaseContext
{
    DbSet<Station> Stations { get; set; }
    DbSet<Consept> Consepts { get; set; }
    DbSet<District> Districts { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Region> Regions { get; set; }
    DbSet<BusinessType> BusinessTypes { get; set; }
    DbSet<Machine> Machines { get; set; }
    DbSet<Payment> Payments { get; set; }
}
