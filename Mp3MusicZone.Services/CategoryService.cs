namespace Mp3MusicZone.Services
{
	using AutoMapper.QueryableExtensions;
	using Contracts;
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class CategoryService : ICategoryService
	{
		private readonly Mp3MusicZoneDbContext context;

		public CategoryService(Mp3MusicZoneDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<CategoryListingServiceModel>> GetAllAsync()
			=> await this.context.Categories
				.OrderBy(c => c.Id)
				.ProjectTo<CategoryListingServiceModel>()
				.ToListAsync();
	}
}
