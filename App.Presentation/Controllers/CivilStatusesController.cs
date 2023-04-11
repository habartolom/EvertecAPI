using App.Application.Models.Contracts;
using App.Application.Services.CivilStatuses;
using App.Application.Services.Users;
using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CivilStatusesController : ControllerBase
	{
		private readonly ICivilStatusService _civilStatusService;
		public CivilStatusesController(ICivilStatusService civilStatusService)
		{
			_civilStatusService = civilStatusService;
		}

		/// <summary>
		/// Endpoint for get all civil statuses
		/// </summary>
		/// <returns>List with all civil statuses</returns>
		[HttpGet("All")]
		public TypedResponseContract<List<CivilStatus>> GetAll()
		{
			return _civilStatusService.GetAllCivilStatuses();
		}
	}
}
