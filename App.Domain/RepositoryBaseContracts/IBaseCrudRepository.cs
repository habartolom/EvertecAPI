namespace App.Domain.RepositoryBaseContracts
{
	public interface IBaseCrudRepository<TEntity> : ICreateRepository<TEntity>, IReadRepository<TEntity>, IUpdateRepository<TEntity>, IDeleteRepository<TEntity> where TEntity : class
	{
		Task SaveChangesAsync();
		void SaveChanges();
	}
}
