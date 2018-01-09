namespace Mp3MusicZone.Services.Contracts
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ISongService
    {
		Task<IEnumerable<SongListingServiceModel>> GetAllAsync(int page = 1);

		Task<int> TotalAsync();

	}
}
