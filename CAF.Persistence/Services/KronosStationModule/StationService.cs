using CAF.Persistence.Contexts;
using Kronos.StationModule.Contexts;
using Kronos.StationModule.Contexts.Common;
using Kronos.StationModule.Domain.Entities;
using Kronos.StationModule.Domain.Module;
using Kronos.StationModule.Services;
using Microsoft.EntityFrameworkCore;

namespace CAF.Persistence.Services.KronosStationModule
{
    public class StationService : IStationService
    {
        private readonly AutomationDbContext _street;
        private readonly AlpetDbContext _alpet;
        private readonly AytemizDbContext _aytemiz;
        private readonly CAFDbContext _main;
        public StationService(AutomationDbContext street, AlpetDbContext alpet, AytemizDbContext aytemiz, CAFDbContext main)
        {
            _street = street;
            _alpet = alpet;
            _aytemiz = aytemiz;
            _main = main;
        }



        public Task<Station> GetStationById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Station>> GetStations()
        {
            var totalListStation = new List<Station>();

            var alpetList = await GetAllStations(_alpet);
            var streetList = await GetAllStations(_street);
            var aytemizList = await GetAllStations(_aytemiz);

            totalListStation.AddRange(alpetList);
            totalListStation.AddRange(streetList);
            totalListStation.AddRange(aytemizList);

            return totalListStation;
        }

        private async Task<List<Station>> GetAllStations(BaseContext context)
        {
            var stations = await context.Stations
                .Include(s => s.CityFk)
                .Include(s => s.DistrictFk)
                .Include(s => s.BusinessTypeFk)
                .Include(s => s.ConseptFk)
                .ToListAsync();

            return stations;
        }

        public async Task<SyncedStation> GetByStationCodeAsync(int stationCode)
        {
            return await _main.KRONOS_STATION
                        .FirstOrDefaultAsync(x => x.StationCode == stationCode);
        }
        public async Task<List<SyncedStation>> GetStationsAsync()
        {
            return await _main.KRONOS_STATION.ToListAsync();

        }
    }
}
