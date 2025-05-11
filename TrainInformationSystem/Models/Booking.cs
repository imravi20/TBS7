using System.ComponentModel.DataAnnotations;

namespace TrainInfoSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public string PassengerName { get; set; }
        public int TrainId { get; set; }
        public int ClassId { get; set; }
        public DateTime JourneyDate { get; set; }
        public Train Train { get; set; }
        public Class Class { get; set; }
        public PNR PNR { get; set; }
        public string Email { get; set; }  // Added Email property

    }
}
