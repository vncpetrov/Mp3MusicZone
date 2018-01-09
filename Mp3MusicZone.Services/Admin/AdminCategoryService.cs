namespace Mp3MusicZone.Services.Admin
{
	using AutoMapper.QueryableExtensions;
	using Contracts;
	using Data;
	using Data.Models;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class AdminCategoryService : IAdminCategoryService
	{
		private readonly Mp3MusicZoneDbContext context;

		public AdminCategoryService(Mp3MusicZoneDbContext context)
		{
			this.context = context;
		}

		public async Task CreateAsync(string name)
		{
			Category category = new Category() { Name = name };

			await this.context.Categories.AddAsync(category);
			await this.context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			Category category = await this.context.Categories
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category is null)
			{
				return;
			}

			this.context.Categories.Remove(category);
			await this.context.SaveChangesAsync();
		}

		public async Task EditAsync(int id, string name)
		{
			Category category = await this.context.Categories
				.Where(c => c.Id == id)
				.FirstOrDefaultAsync();

			if (category is null)
			{
				return;
			}

			category.Name = name;

			await this.context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(string name)
			=> await this.context.Categories
				.AnyAsync(c => c.Name.ToLower() == name.ToLower());

		public async Task<bool> ExistsAsync(int id)
			=> await this.context.Categories
			.AnyAsync(c => c.Id == id);

		public async Task<AdminCategoryServiceModel> GetByIdAsync(int id)
			=> await this.context.Categories
				.Where(c => c.Id == id)
				.ProjectTo<AdminCategoryServiceModel>()
				.FirstOrDefaultAsync();
	}
}
