using CAF.Persistence.Contexts;
using Kronos.StationModule.Contexts;
using Kronos.StationModule.Contexts.Common;
using Kronos.StationModule.Domain.Module;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.KronosStationModule.Job;

public class StationSyncService
{
    private readonly CAFDbContext _main;
    private readonly AlpetDbContext _alpet; // 1
    private readonly AutomationDbContext _street; // 2
    private readonly AytemizDbContext _aytemiz; // 3
    private readonly BpDbContext _bp; // 4
    private readonly GGGDbContext _ggg; // 5
    private readonly GorpetDbContext _gorpet; // 6
    private readonly ModogluDbContext _modoglu; // 7
    private readonly ShellDbContext _shell; // 8
    private readonly TascoDbContext _tasco; // 9
    private readonly TotalDbContext _total; // 10
    private readonly TpDbContext _tp; // 11

    public StationSyncService(CAFDbContext main, AlpetDbContext alpet, AytemizDbContext aytemiz, AutomationDbContext street, BpDbContext bp, GGGDbContext ggg, GorpetDbContext gorpet, ModogluDbContext modoglu, ShellDbContext shell, TascoDbContext tasco, TpDbContext tp, TotalDbContext total)
    {
        _main = main;
        _alpet = alpet;
        _aytemiz = aytemiz;
        _street = street;
        _bp = bp;
        _ggg = ggg;
        _gorpet = gorpet;
        _modoglu = modoglu;
        _shell = shell;
        _tasco = tasco;
        _tp = tp;
        _total = total;
    }

    public async Task SyncStationsAsync()
    {
        var sources = new List<(string Source, BaseContext Context)>
            {
                ("Alpet", _alpet), // 1
                ("Aytemiz", _aytemiz), // 2
                ("Street", _street), // 3
                ("BP", _bp), // 4
                ("GGG", _ggg), // 5
                ("Gorpet", _gorpet), // 6
                ("Modoglu", _modoglu), // 7
                ("Shell", _shell), // 8
                ("Tasco", _tasco),  // 9
                ("Total", _total), // 10
                ("TP", _tp) // 11
            };

        foreach (var (source, context) in sources)
        {
            var stations = await context.Stations
                .Select(s => new
                {
                    s.StationCode,
                    s.StationName,
                    s.SetupDate,
                    s.Platform,
                    s.ServiceTypes
                })
                .ToListAsync();

            foreach (var s in stations)
            {
                var existing = await _main.KRONOS_STATION.FirstOrDefaultAsync(x => x.StationCode == s.StationCode);

                int distributorId = int.Parse(s.StationCode.ToString().Substring(0, 2));

                if (existing == null)
                {
                    _main.KRONOS_STATION.Add(new SyncedStation
                    {
                        StationCode = s.StationCode,
                        StationName = s.StationName,
                        SetupDate = s.SetupDate,
                        Platform = s.Platform,
                        ServiceTypes = s.ServiceTypes,
                        Source = source,
                        DistributorId = distributorId,
                        LastSynced = DateTime.UtcNow
                    });
                }
                else
                {
                    bool changed =
                        existing.StationName != s.StationName ||
                        existing.SetupDate != s.SetupDate ||
                        existing.Platform != s.Platform ||
                        existing.ServiceTypes != s.ServiceTypes ||
                        existing.Source != source ||
                        existing.DistributorId != distributorId;

                    if (changed)
                    {
                        existing.StationName = s.StationName;
                        existing.SetupDate = s.SetupDate;
                        existing.Platform = s.Platform;
                        existing.ServiceTypes = s.ServiceTypes;
                        existing.Source = source;
                        existing.DistributorId = distributorId;
                        existing.LastSynced = DateTime.UtcNow;
                    }
                }
            }
        }

        await _main.SaveChangesAsync();
    }
}
