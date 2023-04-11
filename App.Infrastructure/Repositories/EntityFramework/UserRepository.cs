using App.Domain.AggregatesModel.UserAggregate;
using App.Infrastructure.Database.Context;
using App.Infrastructure.Database.Entities;
using App.Infrastructure.Repositories.EntityFramework.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories.EntityFramework
{
    public class UserRepository : BaseCrudRepository<UserEntity>, IUserRepository<UserEntity>
    {
        public ApplicationContext Context
        {
            get
            {
                return (ApplicationContext)_Database;
            }
        }

        public UserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
