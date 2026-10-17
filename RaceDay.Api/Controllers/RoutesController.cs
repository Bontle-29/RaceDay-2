using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceRoute = RaceDay.Api.Entities.Route;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/route")]
public class RoutesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public RoutesController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoute(int eventId)
    {
        var route = await _context.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(item =>
                item.EventId == eventId);

        if (route == null)
        {
            return NotFound("Route was not found.");
        }

        return Ok(route);
    }

    [HttpPut]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> SaveRoute(
        int eventId,
        RouteRequest request)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(item =>
                item.EventId == eventId);

        if (route == null)
        {
            route = new RaceRoute
            {
                EventId = eventId
            };

            _context.Routes.Add(route);
        }

        route.StartLocation = request.StartLocation;
        route.FinishLocation = request.FinishLocation;
        route.DistanceKm = request.DistanceKm;
        route.RouteDescription = request.RouteDescription;
        route.MapUrl = request.MapUrl;

        await _context.SaveChangesAsync();

        return Ok(route);
    }
}

public class RouteRequest
{
    public string StartLocation { get; set; } = string.Empty;

    public string FinishLocation { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    public string RouteDescription { get; set; } = string.Empty;

    public string MapUrl { get; set; } = string.Empty;
}
