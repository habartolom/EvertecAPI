using App.Config.DIAutoRegister;
using App.Infrastructure.Database.Context;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Config.Dependencies
{
	public class Container
	{
		public static void Register(IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(Container));

			var configMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AutoMapperProfile());
			});

			var mapper = configMapper.CreateMapper();

			services.AddSingleton(mapper);

			services.AddDbContext<ApplicationContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("App.Presentation"));
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			var assembliesToScan = new[]
				{
					Assembly.GetExecutingAssembly(),
					Assembly.Load("App.Domain"),
					Assembly.Load("App.Infrastructure"),
					Assembly.Load("App.Application")
				};

			services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
					.Where(c => c.Name.EndsWith("Repository") ||
						   c.Name.EndsWith("Service") ||
						   c.Name.EndsWith("Validator") ||
						   c.Name.EndsWith("Resource"))
					.AsPublicImplementedInterfaces();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
				{
					builder.AllowAnyHeader();
					builder.AllowAnyMethod();
					builder.AllowAnyOrigin();
				});
			});


			services.AddLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddFile("logs/app.log", minimumLevel: LogLevel.Error, isJson: true);
			});
		}
	}
}
