namespace RaceDay.Api.DTOs;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    public decimal EntryFee { get; set; }

    public int MaximumEntries { get; set; }
}
