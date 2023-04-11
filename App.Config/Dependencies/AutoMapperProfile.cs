using App.Application.Models.Contracts;
using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Domain.AggregatesModel.UserAggregate;
using App.Infrastructure.Database.Entities;
using AutoMapper;

namespace App.Config.Dependencies
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<UserEntity, User>()
				.ReverseMap();

			CreateMap<UserEntity, UserCreateRequestContract>()
				.ReverseMap();

			CreateMap<CivilStatusEntity, CivilStatus>()
				.ReverseMap();
		}
	}
}
