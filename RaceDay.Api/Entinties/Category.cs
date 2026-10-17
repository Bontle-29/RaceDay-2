namespace RaceDay.Api.Entities;

public class Category
{
    public int CategoryId { get; set; }

    public int EventId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    public decimal EntryFee { get; set; }

    public int MaximumEntries { get; set; }

    // Relationship: this category belongs to one event
    public Event? Event { get; set; }

    // Relationship: this category can have many enrolments
    public List<Enrolment> Enrolments { get; set; } = new();
}

