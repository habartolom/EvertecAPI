using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.AggregatesModel.CivilStatusAggregate
{
	public class CivilStatus
	{
		public int CivilStatusId { get; set; }
		public string Description { get; set; } = null!;
	}
}
