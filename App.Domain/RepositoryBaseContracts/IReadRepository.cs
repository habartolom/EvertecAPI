namespace App.Domain.RepositoryBaseContracts
{
	public interface IReadRepository<TEntity> where TEntity : class
	{
		TEntity FindById(object id);
		Task<TEntity> FindByIdAsync(object id);
		IQueryable<TEntity> GetAll();
		IQueryable<TEntity> GetAllPaged(int pageIndex, int pageSize);
	}
}
