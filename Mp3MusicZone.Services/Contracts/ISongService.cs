namespace Mp3MusicZone.Services.Contracts
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ISongService
	{
		Task<IEnumerable<SongListingServiceModel>> GetAllAsync(int page, string searchTerm);
		
		Task IncrementListeningsAsync(string songName);

		Task<int> TotalAsync(string searchTerm);
	}
}
