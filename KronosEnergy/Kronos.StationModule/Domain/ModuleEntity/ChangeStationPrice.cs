using Kronos.StationModule.Domain.ModuleEntity.Approve;

namespace Kronos.StationModule.Domain.ModuleEntity
{
    public class ChangeStationPrice : BaseApprove
    {
        public int WashPrice { get; set; }
        public int WashTime { get; set; }
        public int FoamPrice { get; set; }
        public int FoamTime { get; set; }


    }
}
