using System;

namespace Backend.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        public DateTime Date { get; set; }
        public int AvailableSeats { get; set; }
        public required string CarModel { get; set; } = "Unknown"; 
        public required string CarColor { get; set; } = "Unknown";  
        public string Status { get; set; } = "Active";  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
