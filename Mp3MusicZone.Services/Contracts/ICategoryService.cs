namespace Mp3MusicZone.Services.Contracts
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ICategoryService
    {
		Task<IEnumerable<CategoryListingServiceModel>> GetAllAsync();
	}
}
