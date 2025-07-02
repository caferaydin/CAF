using Kronos.StationModule.Domain.Entities;
using Kronos.StationModule.Domain.Module;
using Kronos.StationModule.Model.Requests;

namespace Kronos.StationModule.Services
{
    public interface IStationService
    {
        Task<List<SyncedStation>> GetStationsAsync();
        Task<SyncedStation> GetByStationCodeAsync(int stationCode);

        Task<(List<SyncedStation> Items, int TotalCount)> GetPagedStationsAsync(StationPagedRequest input);
    }
}
