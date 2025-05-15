using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using RouteModel = Backend.Models.Route; 

[Route("api/trips")] 
[ApiController]
public class TripsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TripsController(AppDbContext context)
    {
        _context = context;
    }

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteModel>>> GetTrips()
    {
        return await _context.Routes.ToListAsync();
    }


    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<RouteModel>>> GetUserTrips(int userId)
    {
        return await _context.Routes.Where(t => t.DriverId == userId).ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<RouteModel>> GetRouteById(int id)
    {
        var route = await _context.Routes.FindAsync(id);
        if (route == null)
        {
            return NotFound(new { message = "Ուղևորությունը չի գտնվել" });
        }
        return Ok(route);
    }


[HttpGet("search")]
public async Task<ActionResult<IEnumerable<RouteModel>>> SearchTrips([FromQuery] string from, [FromQuery] string to)
{
    if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
    {
        return BadRequest(new { message = "Պետք է նշել 'Որտեղից' և 'Ուր' արժեքները:" });
    }

    from = Uri.UnescapeDataString(from);
    to = Uri.UnescapeDataString(to);

    var trips = await _context.Routes
        .Where(t => EF.Functions.ILike(t.From, from) && EF.Functions.ILike(t.To, to)) // ILIKE делает поиск без учета регистра
        .OrderBy(t => t.Date)
        .ToListAsync();

    return Ok(trips);
}


    [HttpPost]
public async Task<IActionResult> CreateRoute([FromBody] RouteModel route)
{
    if (route == null)
        return BadRequest(new { message = "Սխալ տվյալներ" });

    var driver = await _context.Users.FindAsync(route.DriverId);
    if (driver == null)
        return NotFound(new { message = "Վարորդը չի գտնվել" });

    route.Date = DateTime.SpecifyKind(route.Date, DateTimeKind.Utc);
    route.CreatedAt = DateTime.UtcNow;

    _context.Routes.Add(route);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetRouteById), new { id = route.Id }, route);
}
}
