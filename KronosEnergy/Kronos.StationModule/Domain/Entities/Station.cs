using Kronos.StationModule.Domain.Entities.Common;

namespace Kronos.StationModule.Domain.Entities;

public class Station : Entity
{

    public virtual string? StationName { get; set; }

    public virtual string? Adress { get; set; }

    public virtual string? Phone { get; set; }

    public virtual string? Authorized { get; set; }

    public virtual double? X { get; set; }

    public virtual double? Y { get; set; }

    public virtual int PrivateCode { get; set; }

    public virtual string? DealerLicenseCode { get; set; }
                         
    public virtual string? ServiceTypes { get; set; }

    public virtual int StationCode { get; set; }

    public virtual int Platform { get; set; }

    public virtual DateTime SetupDate { get; set; }

    public virtual int CityId { get; set; }

    public City CityFk { get; set; }

    public virtual int DistrictId { get; set; }

    public District DistrictFk { get; set; }

    public virtual int BusinessTypeId { get; set; }

    public BusinessType BusinessTypeFk { get; set; }

    public virtual int ConseptId { get; set; }

    public Consept ConseptFk { get; set; }

}
