using App.Application.Models.Contracts;
using App.Application.Services.Users;
using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Domain.AggregatesModel.UserAggregate;
using App.Infrastructure.Database.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.CivilStatuses
{
	public class CivilStatusService : ICivilStatusService
	{
		private readonly ILogger<CivilStatusService> _logger;
		private readonly IMapper _mapper;
		private readonly ICivilStatusRepository<CivilStatusEntity> _civilStatusRepository;

		public CivilStatusService(ILogger<CivilStatusService> logger, IMapper mapper, ICivilStatusRepository<CivilStatusEntity> civilStatusRepository)
		{
			_logger = logger;
			_mapper = mapper;
			_civilStatusRepository = civilStatusRepository;
		}
		public TypedResponseContract<List<CivilStatus>> GetAllCivilStatuses()
		{
			TypedResponseContract<List<CivilStatus>> response = new TypedResponseContract<List<CivilStatus>>();
			try
			{
				List<CivilStatusEntity> civilStatusEntities = _civilStatusRepository.GetAll().ToList();
				response.Data = _mapper.Map<List<CivilStatus>>(civilStatusEntities);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred!");
				response.Header.Code = HttpStatusCode.InternalServerError;
				response.Header.Message = ex.ToString();
			}
			return response;
		}
	}
}
