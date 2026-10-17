using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet("events/{eventId:int}/categories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories(int eventId)
    {
        var categories = await _context.Categories
            .Where(category => category.EventId == eventId)
            .AsNoTracking()
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("categories/{categoryId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(item =>
                item.CategoryId == categoryId);

        if (category == null)
        {
            return NotFound("Category was not found.");
        }

        return Ok(category);
    }

    [HttpPost("events/{eventId:int}/categories")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> CreateCategory(
        int eventId,
        CreateCategoryRequest request)
    {
        var raceEvent = await _context.Events
            .FirstOrDefaultAsync(item => item.EventId == eventId);

        if (raceEvent == null)
        {
            return NotFound("Event was not found.");
        }

        var category = new Entities.Category
        {
            EventId = eventId,
            Name = request.Name,
            DistanceKm = request.DistanceKm,
            EntryFee = request.EntryFee,
            MaximumEntries = request.MaximumEntries
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCategory),
            new { categoryId = category.CategoryId },
            category);
    }

    [HttpPut("categories/{categoryId:int}")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> UpdateCategory(
        int categoryId,
        CreateCategoryRequest request)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(item =>
                item.CategoryId == categoryId);

        if (category == null)
        {
            return NotFound("Category was not found.");
        }

        category.Name = request.Name;
        category.DistanceKm = request.DistanceKm;
        category.EntryFee = request.EntryFee;
        category.MaximumEntries = request.MaximumEntries;

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("categories/{categoryId:int}")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var category = await _context.Categories
            .Include(item => item.Enrolments)
            .FirstOrDefaultAsync(item =>
                item.CategoryId == categoryId);

        if (category == null)
        {
            return NotFound("Category was not found.");
        }

        if (category.Enrolments.Count > 0)
        {
            return Conflict(
                "A category with enrolments cannot be deleted.");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

