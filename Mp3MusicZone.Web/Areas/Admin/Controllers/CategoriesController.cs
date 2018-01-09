namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Infrastructure.Extensions;
	using Infrastructure.Filters;
	using Services.Admin.Contracts;
	using Services.Admin.Models;
	using Services.Contracts;
	using Services.Models;
	using System;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using AutoMapper;

	public class CategoriesController : BaseAdminController
	{
		private readonly IAdminCategoryService adminCategoryService;
		private readonly ICategoryService categoryService;

		public CategoriesController(
			IAdminCategoryService adminCategoryService,
			ICategoryService categoryService)
		{
			this.adminCategoryService = adminCategoryService;
			this.categoryService = categoryService;
		}

		public IActionResult Create() => View();

		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> Create(CategoryFormModel model)
		{
			if (await this.adminCategoryService.ExistsAsync(model.Name))
			{
				TempData.AddErrorMessage($"Category '{model.Name}' already exists.");

				return View(model);
			}

			await this.adminCategoryService.CreateAsync(model.Name);

			TempData.AddSuccessMessage($"Category {model.Name} successfully created.");

			return View();
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<CategoryListingServiceModel> categories =
				await this.categoryService.GetAllAsync();

			return View(categories);
		}

		public async Task<IActionResult> Edit(int id)
		{
			AdminCategoryServiceModel category =
				await this.adminCategoryService.GetByIdAsync(id);

			if (category is null)
			{
				return NotFound();
			}

			return View(Mapper.Map<CategoryFormModel>(category));
		}

		[HttpPost]
		[ValidateModelState]
		public async Task<IActionResult> Edit(int id, CategoryFormModel model)
		{
			if (!await this.adminCategoryService.ExistsAsync(id))
			{
				return BadRequest();
			}

			await this.adminCategoryService.EditAsync(id, model.Name);

			TempData.AddSuccessMessage("The category was successfully edited.");

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			AdminCategoryServiceModel category =
				await this.adminCategoryService.GetByIdAsync(id);

			if (category is null)
			{
				return NotFound();
			}

			return View(Mapper.Map<CategoryFormModel>(category));
		}

		[HttpPost]
		[ActionName("Delete")]
		public async Task<IActionResult> ConfirmDelete(int id)
		{
			if (!await this.adminCategoryService.ExistsAsync(id))
			{
				return BadRequest();
			}

			await this.adminCategoryService.DeleteAsync(id);

			TempData.AddSuccessMessage("The category was successfully deleted.");

			return RedirectToAction(nameof(Index));
		}
	}
}
