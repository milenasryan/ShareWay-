using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RideRequestsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RideRequestsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] RideRequest request)
    {
        if (request == null)
            return BadRequest(new { message = "Սխալ տվյալներ" });

        var route = await _context.Routes.FindAsync(request.RouteId);
        if (route == null)
            return NotFound(new { message = "Ուղևորությունը չի գտնվել" });

        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            return NotFound(new { message = "Օգտատերը չի գտնվել" });

        var rideRequest = new RideRequest
        {
            RouteId = request.RouteId,
            UserId = request.UserId,
            Purpose = request.Purpose,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.RideRequests.Add(rideRequest);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Հայտը հաջողությամբ ուղարկված է" });
    }

 [HttpGet("driver/{driverId}")]
public async Task<IActionResult> GetRideRequestsForDriver(int driverId)
{
    var requests = await _context.RideRequests
        .Include(r => r.Route)
        .Where(r => r.Route != null && r.Route.DriverId == driverId)  
        .Select(r => new {
            r.Id,
            r.RouteId,
            From = r.Route != null ? r.Route.From : "Անհայտ",  
            To = r.Route != null ? r.Route.To : "Անհայտ",     
            PassengerName = r.User != null ? r.User.FullName : "Անհայտ ուղևոր",  
            r.Purpose,
            r.Status,
            r.CreatedAt,
            Date = r.Route != null ? r.Route.Date : (DateTime?)null  
        })
        .ToListAsync();

    return Ok(requests);
}



    // Обновление статуса заявки
    [HttpPut("{requestId}/status")]
public async Task<IActionResult> UpdateRequestStatus(int requestId, [FromBody] string status)
{
    var request = await _context.RideRequests.FindAsync(requestId);
    if (request == null) return NotFound("Request not found");

    request.Status = status;
    await _context.SaveChangesAsync();

    if (status == "Accepted")
    {
        var trip = await _context.Routes.FindAsync(request.RouteId);
        if (trip != null && trip.AvailableSeats > 0)  
        {
            trip.AvailableSeats--;
            await _context.SaveChangesAsync();
        }
    }

    return Ok(new { message = "Status updated successfully" });
}

[HttpPost("decline/{requestId}")]
public async Task<IActionResult> DeclineRideRequest(int requestId)
{
    // Находим заявку
    var request = await _context.RideRequests
        .Include(r => r.Route)
        .FirstOrDefaultAsync(r => r.Id == requestId);

    if (request == null)
        return NotFound("Заявка не найдена");

    // Меняем статус на "Rejected"
    request.Status = "Rejected";

    // Сохраняем изменения
    await _context.SaveChangesAsync();

    return Ok(new { message = "Заявка отклонена" });
}



    // Принятие заявки (пример дополнительного метода)
    [HttpPost("accept/{requestId}")]
    public async Task<IActionResult> AcceptRideRequest(int requestId)
    {
        var request = await _context.RideRequests.Include(r => r.Route).FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null)
            return NotFound("Заявка не найдена");

        if (request.Route.AvailableSeats > 0)
        {
            request.Status = "Accepted";
            request.Route.AvailableSeats--;

            // Удаляем поездку, если мест больше нет
            if (request.Route.AvailableSeats == 0)
            {
                _context.Routes.Remove(request.Route);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Заявка принята" });
        }
        return BadRequest("Нет свободных мест");
    }

    // Заявки, которые отправил пассажир (userId)
   [HttpGet("passenger/{userId}")]
public async Task<IActionResult> GetRideRequestsForPassenger(int userId)
{
    var requests = await _context.RideRequests
        .Include(r => r.Route)
        .Where(r => r.UserId == userId)
        .Select(r => new {
            r.Id,
            r.RouteId,
            From = r.Route != null ? r.Route.From : "Անհայտ",  
            To = r.Route != null ? r.Route.To : "Անհայտ",      
            r.Status,
            r.Purpose,
            r.CreatedAt,
            Date = r.Route != null ? r.Route.Date : (DateTime?)null 
        })
        .ToListAsync();

    return Ok(requests);
}


}
