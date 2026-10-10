using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Models
{
	public class Event
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = "";

		[Required]
		public DateTime Date { get; set; }

		public string Location { get; set; } = "";

		public List<Rsvp> Rsvps { get; } = [];
	}
}

