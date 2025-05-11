using System.Collections.Generic;

namespace TrainInfoSystem.Models
{

    public class Train
    {

        public int TrainId { get; set; }

        public string TrainName { get; set; }

        public string TrainNumber { get; set; }

        public List<Fare> Fare { get; set; }

        public List<Booking> Bookings { get; set; }

    }

}