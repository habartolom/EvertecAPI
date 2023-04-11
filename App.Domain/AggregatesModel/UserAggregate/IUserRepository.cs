using App.Domain.RepositoryBaseContracts;

namespace App.Domain.AggregatesModel.UserAggregate
{
	public interface IUserRepository<TEntity> : IBaseCrudRepository<TEntity> where TEntity : class
	{
	}
}
