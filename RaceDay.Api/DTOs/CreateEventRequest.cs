namespace RaceDay.Api.DTOs;

public class CreateEventRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public string Venue { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Province { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public DateTime RegistrationOpen { get; set; }

    public DateTime RegistrationClose { get; set; }

    public string Status { get; set; } = "Draft";
}
