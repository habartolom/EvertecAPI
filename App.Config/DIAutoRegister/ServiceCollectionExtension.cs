﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Config.DIAutoRegister
{
	public static class ServiceCollectionExtension
	{
		/// <summary>
		/// This finds all the public, non-generic, non-nested classes in an assembly in the provided assemblies.
		/// If no assemblies provided then it scans the assembly that called the method
		/// </summary>
		/// <param name="services">the NET Core dependency injection service</param>
		/// <param name="assemblies">Each assembly you want scanned. If null then scans the the assembly that called the method</param>
		/// <returns></returns>
		public static AutoRegisterData RegisterAssemblyPublicNonGenericClasses(this IServiceCollection services, params Assembly[] assemblies)
		{
			if (assemblies.Length == 0)
				assemblies = new[] { Assembly.GetCallingAssembly() };

			var allPublicTypes = assemblies.SelectMany(x => x.GetExportedTypes()
				.Where(y => y.IsClass && !y.IsAbstract && !y.IsGenericType && !y.IsNested));
			return new AutoRegisterData(services, allPublicTypes);
		}

		/// <summary>
		/// This allows you to filter the classes in some way.
		/// For instance <code>Where(c =\> c.Name.EndsWith("Service")</code> would only register classes who's name ended in "Service"
		/// </summary>
		/// <param name="autoRegData"></param>
		/// <param name="predicate">A function that will take a type and return true if that type should be included</param>
		/// <returns></returns>
		public static AutoRegisterData Where(this AutoRegisterData autoRegData, Func<Type, bool> predicate)
		{
			if (autoRegData == null) throw new ArgumentNullException(nameof(autoRegData));
			autoRegData.TypeFilter = predicate;
			return new AutoRegisterData(autoRegData.Services, autoRegData.TypesToConsider.Where(predicate));
		}

		/// <summary>
		/// This registers the classes against any public interfaces (other than IDisposable) implemented by the class
		/// </summary>
		/// <param name="autoRegData">AutoRegister data produced by <see cref="RegisterAssemblyPublicNonGenericClasses"/></param> method
		/// <param name="lifetime">Allows you to define the lifetime of the service - defaults to ServiceLifetime.Transient</param>
		/// <returns></returns>
		public static IServiceCollection AsPublicImplementedInterfaces(this AutoRegisterData autoRegData,
			ServiceLifetime lifetime = ServiceLifetime.Transient)
		{
			if (autoRegData == null) throw new ArgumentNullException(nameof(autoRegData));
			foreach (var classType in (autoRegData.TypeFilter == null
				? autoRegData.TypesToConsider
				: autoRegData.TypesToConsider.Where(autoRegData.TypeFilter)))
			{
				var interfaces = classType.GetTypeInfo().ImplementedInterfaces;
				foreach (var infc in interfaces.Where(i => i != typeof(IDisposable) && i.IsPublic && !i.IsNested))
				{
					autoRegData.Services.Add(new ServiceDescriptor(infc, classType, lifetime));
				}
			}

			return autoRegData.Services;
		}
	}
}
