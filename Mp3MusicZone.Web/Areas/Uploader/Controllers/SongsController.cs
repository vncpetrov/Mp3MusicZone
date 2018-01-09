namespace Mp3MusicZone.Web.Areas.Uploader.Controllers
{
	using Data.Models;
	using Infrastructure.Extensions;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Models;
	using Services.Contracts;
	using Services.Models;
	using Services.Uploader.Contracts;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using static Common.Constants.WebConstants;
	using static Common.Constants.ServiceConstants;

	public class SongsController : BaseUploaderController
	{
		private readonly IUploaderSongService songService;
		private readonly ICategoryService categoryService;
		private readonly UserManager<User> userManager;

		public SongsController(
			IUploaderSongService songService,
			ICategoryService categoryService,
			UserManager<User> userManager)
		{
			this.songService = songService;
			this.categoryService = categoryService;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Upload()
		{
			IEnumerable<SelectListItem> categoriesListItems =
				await this.GetCategoriesListItemsAsync();

			return View(new SongFormModel()
			{
				Categories = categoriesListItems
			});
		}

		[HttpPost]
		public async Task<IActionResult> Upload(SongFormModel model)
		{
			model.Categories = await this.GetCategoriesListItemsAsync();

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (model.Song is null)
			{
				TempData.AddErrorMessage("Please, choose a file!");

				return View(model);
			}

			if (!model.Song.FileName.EndsWith($".{SongExtension}")
				|| model.Song.Length > SongFileMaxLength)
			{
				TempData.AddErrorMessage($"Your submission should be a .{SongExtension} file and no more than 10 MBs in size!");

				return View(model);
			}

			if (await this.songService.ExistsAsync(model.Name))
			{
				TempData.AddErrorMessage($"Song {model.Name} already exists!");

				return View(model);
			}

			string userId = this.userManager.GetUserId(User);
			byte[] fileContent = await model.Song.ToByteArrayAsync();

			bool success = await this.songService.UploadAsync(
				userId,
				model.Name,
				model.Singer,
				model.CategoryId,
				fileContent);

			if (!success)
			{
				return BadRequest();
			}

			TempData.AddSuccessMessage($"Song {model.Name}.{SongExtension} uploaded successfully.");

			return View(model);
		}

		private async Task<IEnumerable<SelectListItem>> GetCategoriesListItemsAsync()
		{
			IEnumerable<CategoryListingServiceModel> categories =
				await this.categoryService.GetAllAsync();

			IEnumerable<SelectListItem> categoriesListItems = categories
				.Select(c => new SelectListItem()
				{
					Text = c.Name,
					Value = c.Id.ToString()
				})
				.ToList();

			return categoriesListItems;
		}
	}
}
