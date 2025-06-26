using Kronos.StationModule.Domain.Entities.Common;

namespace Kronos.StationModule.Domain.Entities;

public class District : Entity
{

    public virtual string DistrictName { get; set; }

    public virtual int CityId { get; set; }

    public City CityFk { get; set; }

}
