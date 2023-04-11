using App.Application.Models.Contracts;
using App.Domain.AggregatesModel.CivilStatusAggregate;
using App.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services.CivilStatuses
{
	public interface ICivilStatusService
	{
		TypedResponseContract<List<CivilStatus>> GetAllCivilStatuses();
	}
}
