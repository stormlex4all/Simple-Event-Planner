using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models;

public class RsvpFormModel
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public RsvpStatus Status { get; set; } = RsvpStatus.Going;
}

public class RsvpPageViewModel
{
    public Event Event { get; set; } = new();
    public RsvpSummary Summary { get; set; } = new(0, 0, 0);
    public RsvpFormModel Form { get; set; } = new();
}