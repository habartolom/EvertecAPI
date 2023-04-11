namespace App.Infrastructure.Database.Entities
{
	public partial class CivilStatusEntity
	{
		public int CivilStatusId { get; set; }

		public string Description { get; set; } = null!;

		public virtual ICollection<UserEntity> Users { get; } = new List<UserEntity>();
	}
}
