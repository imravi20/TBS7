using System.Collections.Generic;

namespace TrainInfoSystem.Models
{

    public class Class
    {

        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public List<Fare> Fare { get; set; }
    }
}