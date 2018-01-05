namespace Mp3MusicZone.Services.Admin.Contracts
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IAdminUserService
    {
		Task<IEnumerable<AdminUserListingServiceModel>> GetAllAsync(int page,
			string searchTerm);

		Task<bool> AddToRoleAsync(string userId, string roleName);

		Task<bool> RemoveFromRoleAsync(string userId, string roleName);

		Task<int> TotalAsync(string searchTerm);
	}
}
