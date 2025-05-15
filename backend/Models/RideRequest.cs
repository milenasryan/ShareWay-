using Backend.Models;
using RouteModel = Backend.Models.Route; 

public class RideRequest
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public RouteModel? Route { get; set; } 
    public int UserId { get; set; }
    public User? User { get; set; }
    public string? Purpose { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
