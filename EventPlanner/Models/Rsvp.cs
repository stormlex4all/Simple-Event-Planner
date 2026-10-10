namespace EventPlanner.Models;

public class Rsvp
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RsvpStatus Status { get; set; }
    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
}

public record RsvpSummary(int Going, int Maybe, int Declined);