using System.ComponentModel.DataAnnotations.Schema;
using Kronos.StationModule.Domain.Entities.Common;

namespace Kronos.StationModule.Domain.Entities
{
    public class Machine : Entity
    {
        public virtual int MachineSerialNo { get; set; }

        public virtual int FoamPrice { get; set; }

        public virtual int FoamTime { get; set; }

        public virtual double FoamCounter { get; set; }

        public virtual int WashPrice { get; set; }

        public virtual int WashTime { get; set; }

        public virtual double WashCounter { get; set; }

        public virtual int MachineType { get; set; }

        public virtual DateTime? LastActivation { get; set; }

        public virtual bool IsCoin { get; set; }

        public virtual int CoinPrice { get; set; }

        public virtual int StationId { get; set; }

        public Station StationFk { get; set; }

    }
}
