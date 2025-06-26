using Kronos.StationModule.Domain.Entities;
using Kronos.StationModule.Domain.Module;

namespace Kronos.StationModule.Services
{
    public interface IStationService
    {
        Task<List<Station>> GetStations();
        Task<Station> GetStationById(int id);

        Task<List<SyncedStation>> GetStationsAsync();
        Task<SyncedStation> GetByStationCodeAsync(int stationCode);
    }
}
