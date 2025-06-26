using Kronos.StationModule.Domain.Entities;

namespace CAF.WEB.Models
{
    public class FatihModel
    {
        public List<Station> Station { get; set; } = new List<Station> { }; 
        public List<Station> Machine { get; set; } = new List<Station> { }; 
    }
}
