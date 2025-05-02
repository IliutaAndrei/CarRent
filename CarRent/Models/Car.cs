using System.ComponentModel.DataAnnotations;

namespace CarRent.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int YearOfFabrication { get; set; }
        public FuelType FuelType { get; set; } 
        public TransmissionType TransmissionType { get; set; } 
        public bool IsAvailable { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pretul trebuie sa fie mai mare decat 0")]
        public decimal PricePerDay { get; set; }
    }
}
