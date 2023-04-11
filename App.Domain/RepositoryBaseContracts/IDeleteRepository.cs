namespace App.Domain.RepositoryBaseContracts
{
	public interface IDeleteRepository<TEntity> where TEntity : class
	{
		void Delete(TEntity entity, bool autoSave = true);
		Task DeleteAsync(TEntity entity, bool autoSave = true);
	}
}
