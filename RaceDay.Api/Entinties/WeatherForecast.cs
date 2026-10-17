namespace RaceDay.Api.Entities;

public class WeatherForecast
{
    public int WeatherForecastId { get; set; }

    public int EventId { get; set; }

    public DateTime ForecastDate { get; set; }

    public decimal TemperatureC { get; set; }

    public string Conditions { get; set; } = string.Empty;

    public decimal WindSpeedKmh { get; set; }

    public int RainChancePercent { get; set; }

    public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;

    public Event? Event { get; set; }
}
