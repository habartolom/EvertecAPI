using App.Domain.AggregatesModel.CivilStatusAggregate;

namespace App.Domain.AggregatesModel.UserAggregate
{
	public class User
	{
		public Guid UserId { get; set; }
		public string Name { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public DateTime Birthdate { get; set; }
		public string? PhotoUrl { get; set; }
		public CivilStatus CivilStatus { get; set; } = null!;
		public bool HasSilbings { get; set; }
	}
}
