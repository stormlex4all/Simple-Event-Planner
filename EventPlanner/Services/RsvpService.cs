using System.Net.Mail;
using EventPlanner.Models;

namespace EventPlanner.Services;

public interface IRsvpService
{
    Event? FindEvent(int eventId);
    Rsvp AddRsvp(int eventId, string? name, string? email, RsvpStatus status);
    RsvpSummary GetSummary(int eventId);
}

/// <summary>FR03 Guest RSVP.</summary>
public class RsvpService : IRsvpService
{

    public Event FindEvent(int eventId) => EventStore.Events.Find(e => e.Id == eventId) ?? throw new KeyNotFoundException($"Event {eventId} was not found.");

    /// <summary>
    /// The system shall allow a guest to RSVP with a name, email and response (Going, Maybe, Declined).
    /// A second RSVP from the same email updates the earlier answer instead of adding a duplicate.
    /// </summary>
    public Rsvp AddRsvp(int eventId, string? name, string? email, RsvpStatus status)
    {
        var item = FindEvent(eventId);

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(name)) errors.Add("Name is required.");
        if (!IsValidEmail(email)) errors.Add("A valid email address is required.");
        if (!Enum.IsDefined(status)) errors.Add("Response must be Going, Maybe or Declined.");
        if (errors.Count > 0) throw new ArgumentException(string.Join(" ", errors));

        var rsvp = new Rsvp
        {
            Name = name!.Trim(),
            Email = email!.Trim().ToLowerInvariant(),
            Status = status
        };

        var index = item.Rsvps.FindIndex(r => r.Email == rsvp.Email);
        if (index >= 0) item.Rsvps[index] = rsvp;
        else item.Rsvps.Add(rsvp);
        return rsvp;
    }

    public RsvpSummary GetSummary(int eventId)
    {
        var item = FindEvent(eventId);
        lock (item.Rsvps)
        {
            return new RsvpSummary(
                item.Rsvps.Count(r => r.Status == RsvpStatus.Going),
                item.Rsvps.Count(r => r.Status == RsvpStatus.Maybe),
                item.Rsvps.Count(r => r.Status == RsvpStatus.Declined));
        }
    }

    private static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email)
        && MailAddress.TryCreate(email.Trim(), out var parsed)
        && parsed.Address == email.Trim()
        && email.Contains('.');
}
