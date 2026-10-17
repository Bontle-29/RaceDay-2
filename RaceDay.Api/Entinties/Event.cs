namespace RaceDay.Api.Entities;

public class Event
{
    public int EventId { get; set; }

    public int OrganiserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public string Venue { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Province { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public DateTime RegistrationOpen { get; set; }

    public DateTime RegistrationClose { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? Organiser { get; set; }

    public List<Category> Categories { get; set; } = new();

    public List<WeatherForecast> WeatherForecasts { get; set; } = new();

    public Route? Route { get; set; }

}

