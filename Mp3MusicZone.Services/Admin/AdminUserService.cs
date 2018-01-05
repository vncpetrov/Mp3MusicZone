namespace Mp3MusicZone.Services.Admin
{
	using AutoMapper;
	using Contracts;
	using Data;
	using Data.Models;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using static Common.Constants.WebConstants;

	public class AdminUserService : IAdminUserService
	{
		private readonly Mp3MusicZoneDbContext context;
		private readonly UserManager<User> userManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public AdminUserService(
			Mp3MusicZoneDbContext context,
			UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			this.context = context;
			this.userManager = userManager;
			this.roleManager = roleManager;
		}

		public async Task<IEnumerable<AdminUserListingServiceModel>> GetAllAsync(
			int currentPage = 1, string searchTerm = null)
		{
			//IEnumerable<User> users = await this.context.Users
			//	.OrderBy(u => u.UserName)
			//	.Skip((currentPage - 1) * DefaultPageSize)
			//	.Take(DefaultPageSize)
			//	.ToListAsync();

			IQueryable<User> usersAsQueryable = this.context.Users
				.OrderBy(u => u.UserName)
				.AsQueryable();

			if (searchTerm != null)
			{
				usersAsQueryable = usersAsQueryable
					.Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower()));
			}

			IEnumerable<User> users = await usersAsQueryable
					.Skip((currentPage - 1) * DefaultPageSize)
					.Take(DefaultPageSize)
					.ToListAsync();

			List<AdminUserListingServiceModel> userListings =
				new List<AdminUserListingServiceModel>();

			foreach (var user in users)
			{
				IEnumerable<string> userRoles = await this.userManager.GetRolesAsync(user);

				AdminUserListingServiceModel userListing =
					Mapper.Map<AdminUserListingServiceModel>(user);
				userListing.Roles = userRoles;

				userListings.Add(userListing);
			}

			return userListings;
		}

		public async Task<bool> AddToRoleAsync(string userId, string roleName)
		{
			bool roleExists = await this.roleManager.RoleExistsAsync(roleName);
			User user = await this.userManager.FindByIdAsync(userId);

			if (!roleExists || user is null)
			{
				return false;
			}

			IdentityResult result = await this.userManager.AddToRoleAsync(user, roleName);

			return result.Succeeded;
		}

		public async Task<bool> RemoveFromRoleAsync(string userId, string roleName)
		{
			bool roleExists = await this.roleManager.RoleExistsAsync(roleName);
			User user = await this.userManager.FindByIdAsync(userId);

			if (!roleExists || user is null)
			{
				return false;
			}

			IdentityResult result = await this.userManager.RemoveFromRoleAsync(user, roleName);

			return result.Succeeded;
		}

		public async Task<int> TotalAsync(string searchTerm = null)
		{
			searchTerm = searchTerm ?? string.Empty;

			return await this.context.Users
				.Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower()))
				.CountAsync();
		}

	}
}
