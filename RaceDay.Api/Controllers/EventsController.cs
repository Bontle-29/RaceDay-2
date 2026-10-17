using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    // GET: /api/events
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events
            .AsNoTracking()
            .ToListAsync();

        return Ok(events);
    }

    // GET: /api/events/1
    [HttpGet("{eventId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvent(int eventId)
    {
        var selectedEvent = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(raceEvent =>
                raceEvent.EventId == eventId);

        if (selectedEvent == null)
        {
            return NotFound("Event was not found.");
        }

        return Ok(selectedEvent);
    }
}

