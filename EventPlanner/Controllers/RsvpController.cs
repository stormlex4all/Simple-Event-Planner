using EventPlanner.Models;
using EventPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Controllers;

// FR03 - Guest RSVP
public class RsvpController : Controller
{
    private readonly IRsvpService _rsvps;

    public RsvpController(IRsvpService rsvps) => _rsvps = rsvps;

    [HttpGet]
    public IActionResult Create(int eventId)
    {
        var page = BuildPage(eventId, new RsvpFormModel());
        return page is null ? NotFound() : View(page);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(int eventId, [Bind(Prefix = "Form")] RsvpFormModel form)
    {
        var page = BuildPage(eventId, form);
        if (page is null) return NotFound();
        if (!ModelState.IsValid) return View(page);

        try
        {
            _rsvps.AddRsvp(eventId, form.Name, form.Email, form.Status);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(page);
        }

        TempData["Message"] = "Thanks! Your RSVP has been recorded.";
        return RedirectToAction(nameof(Create), new { eventId });
    }

    private RsvpPageViewModel? BuildPage(int eventId, RsvpFormModel form)
    {
        var item = _rsvps.FindEvent(eventId);
        if (item is null) return null;
        return new RsvpPageViewModel { Event = item, Summary = _rsvps.GetSummary(eventId), Form = form };
    }
}