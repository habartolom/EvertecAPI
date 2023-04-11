using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Config.DIAutoRegister
{
	public class AutoRegisterData
	{
		/// <summary>
		/// RegisterAssemblyPublicNonGenericClasses uses this to create the initial data
		/// </summary>
		/// <param name="services"></param>
		/// <param name="typesToConsider"></param>
		public AutoRegisterData(IServiceCollection services, IEnumerable<Type> typesToConsider)
		{
			Services = services ?? throw new ArgumentNullException(nameof(services));
			TypesToConsider = typesToConsider ?? throw new ArgumentNullException(nameof(typesToConsider));
		}

		/// <summary>
		/// This carries the service register 
		/// </summary>
		public IServiceCollection Services { get; }

		/// <summary>
		/// This holds the class types found by the RegisterAssemblyPublicNonGenericClasses
		/// </summary>
		public IEnumerable<Type> TypesToConsider { get; }

		/// <summary>
		/// This holds an options test method which will be applied using a Where clause to filter the classes
		/// If the TypeFiler is null, then no filtering is done
		/// </summary>
		public Func<Type, bool> TypeFilter { get; set; }
	}
}
