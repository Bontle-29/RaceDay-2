namespace RaceDay.Api.Entities;

public class Enrolment
{
    public int EnrolmentId { get; set; }

    public int CategoryId { get; set; }

    public int ParticipantId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Active";

    public string RaceNumber { get; set; } = string.Empty;

    // Relationships
    public Category? Category { get; set; }

    public User? Participant { get; set; }

    public Result? Result { get; set; }
}

