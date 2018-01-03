namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
	using Microsoft.Extensions.DependencyInjection;
	using Mp3MusicZone.Services.Contracts;
	using System;
	using System.Linq;
	using System.Reflection;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			var types = Assembly.GetAssembly(typeof(IService))
				.GetTypes()
				.Where(t => t.IsClass
						 && t.IsPublic
						 && !t.IsAbstract
						 && !t.IsInterface
						 && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
				.Select(t => new
				{
					Implementation = t,
					Interface = t.GetInterface($"I{t.Name}")
				})
				.ToList();

			foreach (var type in types)
			{
				services.AddTransient(type.Interface, type.Implementation);
			}

			return services;
		}
	}
}
