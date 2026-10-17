using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Entities;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet("events/{eventId:int}/results")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventResults(int eventId)
    {
        var results = await _context.Results
            .Include(result => result.Enrolment)
            .ThenInclude(enrolment => enrolment!.Category)
            .Where(result =>
                result.Enrolment!.Category!.EventId == eventId &&
                result.ResultStatus == "Published")
            .AsNoTracking()
            .ToListAsync();

        return Ok(results);
    }

    [HttpGet("results/{resultId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetResult(int resultId)
    {
        var result = await _context.Results
            .AsNoTracking()
            .FirstOrDefaultAsync(item =>
                item.ResultId == resultId);

        if (result == null)
        {
            return NotFound("Result was not found.");
        }

        return Ok(result);
    }

    [HttpPost("enrolments/{enrolmentId:int}/result")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> CreateResult(
        int enrolmentId,
        ResultRequest request)
    {
        var enrolment = await _context.Enrolments
            .FirstOrDefaultAsync(item =>
                item.EnrolmentId == enrolmentId);

        if (enrolment == null)
        {
            return NotFound("Enrolment was not found.");
        }

        var existingResult = await _context.Results
            .AnyAsync(item => item.EnrolmentId == enrolmentId);

        if (existingResult)
        {
            return Conflict("A result already exists.");
        }

        var result = new Result
        {
            EnrolmentId = enrolmentId,
            FinishPosition = request.FinishPosition,
            FinishTimeSeconds = request.FinishTimeSeconds,
            ResultStatus = request.ResultStatus,
            RecordedAt = DateTime.UtcNow
        };

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return Created("", result);
    }

    [HttpDelete("results/{resultId:int}")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> DeleteResult(int resultId)
    {
        var result = await _context.Results
            .FirstOrDefaultAsync(item =>
                item.ResultId == resultId);

        if (result == null)
        {
            return NotFound("Result was not found.");
        }

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class ResultRequest
{
    public int FinishPosition { get; set; }

    public int FinishTimeSeconds { get; set; }

    public string ResultStatus { get; set; } = "Published";
}
