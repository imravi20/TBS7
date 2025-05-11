using System.Security.Claims;

namespace TrainInfoSystem.Models
{
    public class Fare
    {
        public int TrainId { get; set; }
        public int ClassId { get; set; }
        public decimal FareAmount { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public Train Train { get; set; }
        public Class Class { get; set; }
    }
}