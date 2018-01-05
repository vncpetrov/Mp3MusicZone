namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
	using Data;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Common.Constants;
	using Data.Models;
	using System;

	using static Common.Constants.WebConstants;
	using System.Threading.Tasks;
	using Mp3MusicZone.Data.Models.Enums;

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

				UserManager<User> userManager =
					serviceScope.ServiceProvider.GetService<UserManager<User>>();
				RoleManager<IdentityRole> roleManager =
					serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

				SeedRoles(roleManager);
				SeedAdministrator(roleManager, userManager);
			}

			return builder;
		}

		private static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			Task.Run(async () =>
			{
				string[] roles = new[]
			{
				AdministratorRole,
				UploaderRole
			};

				foreach (var role in roles)
				{
					bool roleExists = await roleManager.RoleExistsAsync(role);

					if (!roleExists)
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}
			})
			.GetAwaiter()
			.GetResult();
		}

		private static void SeedAdministrator(RoleManager<IdentityRole> roleManager,
			UserManager<User> userManager)
		{
			Task.Run(async () =>
			{
				string adminName = AdministratorRole;

				User adminUser = await userManager.FindByNameAsync(adminName);

				if (adminUser is null)
				{
					adminUser = new User()
					{
						UserName = adminName,
						Email = "mp3musiczone.info@gmail.com",
						EmailConfirmed = true,
						FirstName = "Mp3MusicZone",
						LastName = adminName,
						Birthdate = new DateTime(1990, 4, 16),
						Genre = GenreType.Male,
					};

					await userManager.CreateAsync(adminUser, "Test12");

					await userManager.AddToRoleAsync(adminUser, adminName);
				}

			})
			.GetAwaiter()
			.GetResult();
		}
	}
}
