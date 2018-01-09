namespace Mp3MusicZone.Services.Admin.Contracts
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IAdminCategoryService
    {
		Task CreateAsync(string name);

		Task<bool> ExistsAsync(string name);
		Task<bool> ExistsAsync(int id);
		
		Task<AdminCategoryServiceModel> GetByIdAsync(int id);

		Task EditAsync(int id, string name);

		Task DeleteAsync(int id);
	}
}
