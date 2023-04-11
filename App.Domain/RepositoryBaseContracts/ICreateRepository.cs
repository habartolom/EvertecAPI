namespace App.Domain.RepositoryBaseContracts
{
	public interface ICreateRepository<T> where T : class
	{
		Task<IEnumerable<T>> BulkCreateAsync(IEnumerable<T> entities, bool autoSave = true);
		T Create(T entity, bool autoSave = true);
		Task<T> CreateAsync(T entity, bool autoSave = true);
	}
}
