namespace App.Domain.RepositoryBaseContracts
{
	public interface IUpdateRepository<TEntity> where TEntity : class
	{
		Task<IEnumerable<TEntity>> BulkUpdateAsync(IEnumerable<TEntity> entities, bool autoSave = true);
		TEntity Update(TEntity entity, bool autoSave = true);
		Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true);
	}
}
