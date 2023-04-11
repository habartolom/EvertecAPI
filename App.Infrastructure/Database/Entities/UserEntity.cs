namespace App.Infrastructure.Database.Entities
{
	public partial class UserEntity
	{
		public Guid UserId { get; set; }

		public string Name { get; set; } = null!;

		public string Lastname { get; set; } = null!;

		public DateTime Birthdate { get; set; }

		public string FileName { get; set; } = null!;

		public string UniqueFileName { get; set; } = null!;

		public int CivilStatusId { get; set; }

		public bool HasSilbings { get; set; }

		public virtual CivilStatusEntity CivilStatus { get; set; } = null!;
	}
}
