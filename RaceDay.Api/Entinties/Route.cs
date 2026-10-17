namespace RaceDay.Api.Entities;

public class Route
{
    public int RouteId { get; set; }

    public int EventId { get; set; }

    public string StartLocation { get; set; } = string.Empty;

    public string FinishLocation { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    public string RouteDescription { get; set; } = string.Empty;

    public string MapUrl { get; set; } = string.Empty;

    public Event? Event { get; set; }
}
