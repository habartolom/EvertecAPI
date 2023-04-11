using App.Domain.RepositoryBaseContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace App.Infrastructure.Repositories.EntityFramework.Base
{
	public abstract class BaseCrudRepository<TEntity> : IBaseCrudRepository<TEntity>, ICreateRepository<TEntity>, IReadRepository<TEntity>, IUpdateRepository<TEntity>, IDeleteRepository<TEntity> where TEntity : class
	{
		protected DbContext _Database;
		protected DbSet<TEntity> _Table;

		public BaseCrudRepository(DbContext context)
		{
			_Database = context;
			_Table = _Database.Set<TEntity>();
		}
		public async Task<IEnumerable<TEntity>> BulkCreateAsync(IEnumerable<TEntity> entities, bool autoSave = true)
		{
			await _Table.AddRangeAsync(entities);
			if (autoSave)
				await SaveChangesAsync();

			return entities;
		}

		public async Task<IEnumerable<TEntity>> BulkUpdateAsync(IEnumerable<TEntity> entities, bool autoSave = true)
		{
			foreach (TEntity newItem in entities)
				await UpdateAsync(newItem, autoSave: false);

			if (autoSave)
				await SaveChangesAsync();

			return entities;
		}

		public virtual TEntity Create(TEntity entity, bool autoSave = true)
		{
			_Table.Add(entity);

			if (autoSave)
				_Database.SaveChanges();

			return entity;
		}

		public async Task<TEntity> CreateAsync(TEntity entity, bool autoSave = true)
		{
			await _Table.AddAsync(entity);

			if (autoSave)
				await SaveChangesAsync();

			return entity;
		}

		public virtual void Delete(TEntity entity, bool autoSave = true)
		{
			_Table.Remove(entity);

			if (autoSave)
				_Database.SaveChanges();
		}

		public async Task DeleteAsync(TEntity entity, bool autoSave = true)
		{
			_Table.Remove(entity);

			if (autoSave)
				await SaveChangesAsync();
		}

		public virtual TEntity FindById(object id)
		{
			object obj = CastPrimaryKey(id);
			return _Table.Find(obj);
		}

		public virtual async Task<TEntity> FindByIdAsync(object id)
		{
			object newId = CastPrimaryKey(id);
			return await _Table.FindAsync(newId);
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return _Table;
		}

		public virtual IQueryable<TEntity> GetAllPaged(int pageIndex, int pageSize)
		{
			return _Table.Skip((pageIndex - 1) * pageSize).Take(pageSize);
		}

		public virtual void SaveChanges()
		{
			_Database.SaveChanges();
		}

		public virtual async Task SaveChangesAsync()
		{
			await _Database.SaveChangesAsync();
		}

		public virtual TEntity Update(TEntity entity, bool autoSave = true)
		{
			if (entity == null)
				return null;

			TEntity val = _Table.Find(GetValuePrimaryKey(entity));
			if (val != null)
			{
				_Database.Entry(val).CurrentValues.SetValues(entity);
				if (autoSave)
					_Database.SaveChanges();
			}

			return val;
		}

		public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true)
		{
			TEntity oldItem = await FindByIdAsync(GetValuePrimaryKey(entity));
			_Database.Entry(oldItem).CurrentValues.SetValues(entity);
			if (autoSave)
				await SaveChangesAsync();

			return entity;
		}

		protected object CastPrimaryKey(object id)
		{
			string primaryKeyName = GetPrimaryKeyName();
			Type propertyType = typeof(TEntity).GetProperty(primaryKeyName)!.PropertyType;
			return Convert.ChangeType(id, propertyType);
		}

		protected string GetPrimaryKeyName()
		{
			IEnumerable<string> source = _Database.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties.Select((IProperty x) => x.Name);
			string text = source.FirstOrDefault();
			if (source.Count() > 1)
				throw new ApplicationException("The object has more than 1 primary key. Which does not allow the automation of BaseModel in the Update");

			if (text == null)
				throw new ApplicationException("The object has no primary key. Which does not allow the automation of BaseModel in the Update");

			return text;
		}

		protected object GetValuePrimaryKey(TEntity entity)
		{
			string primaryKeyName = GetPrimaryKeyName();
			return typeof(TEntity).GetProperty(primaryKeyName)!.GetValue(entity);
		}
	}
}
