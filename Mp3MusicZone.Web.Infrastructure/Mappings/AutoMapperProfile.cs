namespace Mp3MusicZone.Web.Infrastructure.Mappings
{
	using AutoMapper;
	using Mp3MusicZone.Common.Mappings;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			IEnumerable<Type> allTypes = this.GetTypes();

			this.RegisterStandartMappings(allTypes, this);
			this.RegisterCustomMappings(allTypes, this);
		}

		private IEnumerable<Type> GetTypes()
		{
			List<Type> types = AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(a => a.GetName().Name.Contains("Mp3MusicZone"))
				.SelectMany(a => a.GetTypes())
				.Where(t => t.IsClass
						 && t.GetInterfaces().Any(i => i.IsGenericType
							 && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
				.ToList();

			return types;
		}

		private void RegisterStandartMappings(IEnumerable<Type> types, AutoMapperProfile profile)
		{
			var standartMappings = types
				.Select(t => new
				{
					Destination = t,
					Source = t.GetInterfaces()
						.First(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
						.GetGenericArguments()
						.First()
				})
				.ToList();

			foreach (var mapping in standartMappings)
			{
				this.CreateMap(mapping.Source, mapping.Destination);
			}
		}

		private void RegisterCustomMappings(IEnumerable<Type> types, AutoMapperProfile profile)
		{
			var customMappings = types.Where(t =>
					t.IsClass
					&& !t.IsAbstract
					&& !t.IsInterface
					&& typeof(IHaveCustomMappings).IsAssignableFrom(t))
				.Select(t => (IHaveCustomMappings)Activator.CreateInstance(t))
				.ToList();

			foreach (var mapping in customMappings)
			{
				mapping.ConfigureMappings(this);
			}
		}
	}
}
