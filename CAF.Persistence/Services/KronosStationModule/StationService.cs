using CAF.Persistence.Contexts;
using Kronos.StationModule.Contexts;
using Kronos.StationModule.Contexts.Common;
using Kronos.StationModule.Domain.Entities;
using Kronos.StationModule.Domain.Module;
using Kronos.StationModule.Model.Requests;
using Kronos.StationModule.Services;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.KronosStationModule
{
    public class StationService : IStationService
    {
        private readonly CAFDbContext _main;

        public StationService(CAFDbContext main)
        {
            _main = main;
        }

        public async Task<SyncedStation> GetByStationCodeAsync(int stationCode)
        {
            return await _main.KRONOS_STATION
                        .FirstOrDefaultAsync(x => x.StationCode == stationCode);
        }

        public async Task<(List<SyncedStation> Items, int TotalCount)> GetPagedStationsAsync(StationPagedRequest input)
        {
            var query = _main.KRONOS_STATION.AsNoTracking();

            // Arama filtresi varsa uygula
            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                query = query.Where(x =>
                    x.StationName.Contains(input.Search) ||
                    x.StationCode.ToString().Contains(input.Search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Id) // sıralama burada sabit veya dinamik yapılabilir
                .Skip(input.Start)
                .Take(input.Length)
                .Select(x => new SyncedStation
                {
                    Id = x.Id,
                    StationName = x.StationName,
                    StationCode = x.StationCode,
                    ServiceTypes = x.ServiceTypes,
                    Platform = x.Platform,
                    LastSynced = x.LastSynced
                })
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<SyncedStation>> GetStationsAsync()
        {
            return await _main.KRONOS_STATION.ToListAsync();

        }
    }
}
