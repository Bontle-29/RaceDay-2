using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Entities;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/weather")]
public class WeatherController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public WeatherController(RaceDayDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetWeather(int eventId)
    {
        var forecasts = await _context.WeatherForecasts
            .Where(item => item.EventId == eventId)
            .OrderBy(item => item.ForecastDate)
            .AsNoTracking()
            .ToListAsync();

        return Ok(forecasts);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> AddWeather(
        int eventId,
        WeatherRequest request)
    {
        var forecast = new WeatherForecast
        {
            EventId = eventId,
            ForecastDate = request.ForecastDate,
            TemperatureC = request.TemperatureC,
            Conditions = request.Conditions,
            WindSpeedKmh = request.WindSpeedKmh,
            RainChancePercent = request.RainChancePercent,
            RetrievedAt = DateTime.UtcNow
        };

        _context.WeatherForecasts.Add(forecast);
        await _context.SaveChangesAsync();

        return Created("", forecast);
    }

    [HttpDelete("/api/weather/{weatherForecastId:int}")]
    [Authorize(Roles = "Organiser,Admin")]
    public async Task<IActionResult> DeleteWeather(
        int weatherForecastId)
    {
        var forecast = await _context.WeatherForecasts
            .FirstOrDefaultAsync(item =>
                item.WeatherForecastId == weatherForecastId);

        if (forecast == null)
        {
            return NotFound("Weather forecast was not found.");
        }

        _context.WeatherForecasts.Remove(forecast);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class WeatherRequest
{
    public DateTime ForecastDate { get; set; }

    public decimal TemperatureC { get; set; }

    public string Conditions { get; set; } = string.Empty;

    public decimal WindSpeedKmh { get; set; }

    public int RainChancePercent { get; set; }
}

