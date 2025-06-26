using Kronos.StationModule.Domain.Entities.Common;

namespace Kronos.StationModule.Domain.Entities
{
    public class Payment : Entity
    {

        public virtual string Description { get; set; }

        public virtual int WashType { get; set; }

        public virtual int PaymentType { get; set; }

        public virtual double Price { get; set; }

        public virtual int CleanTime { get; set; }

        public virtual int ExpiredTime { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? ReadDate { get; set; }

        public virtual int StationCode { get; set; }

        public virtual int MachineSerialNo { get; set; }

    }
}
