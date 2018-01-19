namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
	using Data.Models;
	using Infrastructure.Extensions;
	using Infrastructure.Filters;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Mp3MusicZone.Web.Models.Shared;
	using Services.Admin.Contracts;
	using Services.Admin.Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using static Common.Constants.WebConstants;

	public class UsersController : BaseAdminController
	{
		private readonly UserManager<User> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IAdminUserService userService;

		public UsersController(
			UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager,
			IAdminUserService userService)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.userService = userService;
		}

		public async Task<IActionResult> Index(int page = 1, string searchTerm = null)
		{
			int totalUsers = await this.userService.TotalAsync(searchTerm);
			int pageSize = DefaultPageSize;
			int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

			if (page < 1)
			{
				page = 1;
			}

			if (totalPages > 0 && page > totalPages)
			{
				page = totalPages;
			}

			IEnumerable<AdminUserListingServiceModel> users =
				await this.userService.GetAllAsync(page, searchTerm);

			return View(new PaginatedSearchViewModel<AdminUserListingServiceModel>()
			{
				SearchTerm = searchTerm,
				PageInfo = new PaginatedViewModel<AdminUserListingServiceModel>
				{
					Items = users,
					Current = page,
					PageSize = pageSize,
					TotalPages = totalPages
				}
			});
		}

		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> AddToRole(UserRoleFormModel model,
			int page, string searchTerm)
		{
			bool result = await this.userService.AddToRoleAsync(model.UserId, model.Role);

			if (!result)
			{
				TempData.AddErrorMessage("Invalid identity details.");
			}
			else
			{
				TempData.AddSuccessMessage($"User successfully added to {model.Role} role.");
			}

			return RedirectToAction(nameof(Index),
				new
				{
					page = page,
					searchTerm = searchTerm
				});
		}

		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> RemoveFromRole(UserRoleFormModel model,
			int page, string searchTerm)
		{
			bool result = await this.userService.RemoveFromRoleAsync(model.UserId, model.Role);

			if (!result)
			{
				TempData.AddErrorMessage("Invalid identity details.");
			}
			else
			{
				TempData.AddSuccessMessage($"User successfully removed from the {model.Role} role.");
			}

			return RedirectToAction(nameof(Index),
				new
				{
					page = page,
					searchTerm = searchTerm
				});
		}
	}
}
