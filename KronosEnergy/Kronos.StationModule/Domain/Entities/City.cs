using Kronos.StationModule.Domain.Entities.Common;

namespace Kronos.StationModule.Domain.Entities;

public class City : Entity
{

    public virtual string CityName { get; set; }

    public virtual int? RegionId { get; set; }

    public Region RegionFk { get; set; }

}
