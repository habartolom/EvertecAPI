using App.Domain.RepositoryBaseContracts;

namespace App.Domain.AggregatesModel.CivilStatusAggregate
{
	public interface ICivilStatusRepository<TEntity> : IBaseCrudRepository<TEntity> where TEntity : class
	{
	}
}
