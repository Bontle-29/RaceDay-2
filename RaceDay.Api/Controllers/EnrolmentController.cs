using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Entities;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet("enrolments/me")]
    [Authorize(Roles = "Participant,Admin")]
    public async Task<IActionResult> GetMyEnrolments()
    {
        var participantId = GetUserId();

        var enrolments = await _context.Enrolments
            .Include(item => item.Category)
            .ThenInclude(category => category!.Event)
            .Where(item => item.ParticipantId == participantId)
            .AsNoTracking()
            .ToListAsync();

        return Ok(enrolments);
    }

    [HttpPost("categories/{categoryId:int}/enrolments")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Enrol(int categoryId)
    {
        var participantId = GetUserId();

        var category = await _context.Categories
            .FirstOrDefaultAsync(item =>
                item.CategoryId == categoryId);

        if (category == null)
        {
            return NotFound("Category was not found.");
        }

        var alreadyEnrolled = await _context.Enrolments
            .AnyAsync(item =>
                item.CategoryId == categoryId &&
                item.ParticipantId == participantId);

        if (alreadyEnrolled)
        {
            return Conflict("You are already enrolled.");
        }

        var numberOfEnrolments = await _context.Enrolments
            .CountAsync(item =>
                item.CategoryId == categoryId &&
                item.Status == "Active");

        if (numberOfEnrolments >= category.MaximumEntries)
        {
            return Conflict("This category is full.");
        }

        var enrolment = new Enrolment
        {
            CategoryId = categoryId,
            ParticipantId = participantId,
            Status = "Active",
            RaceNumber = $"R{numberOfEnrolments + 1:000}",
            EnrolledAt = DateTime.UtcNow
        };

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return Created("", enrolment);
    }

    [HttpPut("enrolments/{enrolmentId:int}/cancel")]
    public async Task<IActionResult> Cancel(int enrolmentId)
    {
        var userId = GetUserId();

        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(item =>
                item.EnrolmentId == enrolmentId);

        if (enrolment == null)
        {
            return NotFound("Enrolment was not found.");
        }

        if (enrolment.ParticipantId != userId &&
            !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        enrolment.Status = "Cancelled";
        await _context.SaveChangesAsync();

        return Ok(enrolment);
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return int.Parse(claim!);
    }
}
