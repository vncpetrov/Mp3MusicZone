namespace Mp3MusicZone.Services
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

	using static Common.Constants.WebConstants;

	public class SongService : ISongService
	{
		private readonly Mp3MusicZoneDbContext context;

		public SongService(Mp3MusicZoneDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<SongListingServiceModel>> GetAllAsync(
			int page = 1,
			string searchTerm = null)
		{
			IQueryable<Song> songsAsQueryable = this.context.Songs
				.OrderBy(s => s.Id)
				.AsQueryable();

			if (searchTerm != null)
			{
				songsAsQueryable = songsAsQueryable
					.Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()));
			}

			IEnumerable<SongListingServiceModel> songs =
				await songsAsQueryable
				.Skip((page - 1) * DefaultPageSize)
				.Take(DefaultPageSize)
				.ProjectTo<SongListingServiceModel>()
				.ToListAsync();

			return songs;
		}

		public async Task IncrementListeningsAsync(string songName)
		{
			Song song = await this.context.Songs
				.FirstOrDefaultAsync(s => s.Name == songName);

			if (song is null)
			{
				return;
			}

			song.Listenings++;
			await this.context.SaveChangesAsync();
		}

		public async Task<int> TotalAsync(string searchTerm = null)
		{
			searchTerm = searchTerm ?? string.Empty;

			return await this.context.Songs
				.Where(s => s.Name.ToLower().Contains(searchTerm.ToLower()))
				.CountAsync();
		}

	}
}
