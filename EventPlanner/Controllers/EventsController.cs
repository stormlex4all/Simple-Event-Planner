using Microsoft.AspNetCore.Mvc;
using EventPlanner.Models;

namespace EventPlanner.Controllers
{
	public class EventsController : Controller
	{
		private static int nextId = 1;

		public IActionResult Index()
		{
			return View(EventStore.Events.OrderBy(e => e.Date).ToList());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Event newEvent)
		{
			if (ModelState.IsValid)
			{
				newEvent.Id = nextId++;
				EventStore.Events.Add(newEvent);

				return RedirectToAction("Index");
			}

			return View(newEvent);
		}
	}
}
