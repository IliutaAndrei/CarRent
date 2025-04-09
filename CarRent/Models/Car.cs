namespace CarRent.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int YearOfFabrication { get; set; }
        public required string FuelType { get; set; }
        public required string TransimissionType { get; set; }
        public bool IsAvailable { get; set; }
        public decimal PricePerDay { get; set; }
    }
}
