namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
	using Data;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using System;

	public static class ApplicationBuilderExtensions
    {
		public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder builder)
		{
			using (IServiceScope serviceScope = builder.ApplicationServices
												   .GetRequiredService<IServiceScopeFactory>()
												   .CreateScope())
			{
				serviceScope.ServiceProvider
					.GetService<Mp3MusicZoneDbContext>()
					.Database
					.Migrate();
			}

			return builder;
		}
    }
}
