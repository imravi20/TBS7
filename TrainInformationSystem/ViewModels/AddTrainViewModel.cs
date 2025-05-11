using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainInfoSystem.ViewModels
{
    public class AddTrainViewModel
    {
        [Required]
        public string TrainName { get; set; }

        [Required]
        public string TrainNumber { get; set; }

        public List<ClassDetails> Classes { get; set; }
    }

    public class ClassDetails
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public bool IsSelected { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fare must be a positive number.")]
        public decimal FareAmount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Total seats must be a positive number.")]
        public int TotalSeats { get; set; }
    }
}