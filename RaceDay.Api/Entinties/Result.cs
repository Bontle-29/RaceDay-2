namespace RaceDay.Api.Entities;

public class Result
{
    public int ResultId { get; set; }

    public int EnrolmentId { get; set; }

    public int FinishPosition { get; set; }

    public int FinishTimeSeconds { get; set; }

    public string ResultStatus { get; set; } = string.Empty;

    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public Enrolment? Enrolment { get; set; }
}
