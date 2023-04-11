using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Infrastructure.Database.Context;
using App.Infrastructure.Database.Entities;
using App.Infrastructure.Repositories.EntityFramework.Base;

namespace App.Infrastructure.Repositories.EntityFramework
{
	public class CivilStatusRepository : BaseCrudRepository<CivilStatusEntity>, ICivilStatusRepository<CivilStatusEntity>
	{
		public ApplicationContext Context
		{
			get
			{
				return (ApplicationContext)_Database;
			}
		}

		public CivilStatusRepository(ApplicationContext context) : base(context)
		{
		}
	}
}
