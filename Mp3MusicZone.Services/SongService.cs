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

	using static Common.Constants.WebConstants;

	public class SongService : ISongService
	{
		private readonly Mp3MusicZoneDbContext context;

		public SongService(Mp3MusicZoneDbContext context)
		{
			this.context = context;
		}
		
		public async Task<IEnumerable<SongListingServiceModel>> GetAllAsync(int page = 1)
		{
			return await this.context.Songs
				.OrderBy(s => s.Id)
				.Skip((page - 1) * DefaultPageSize)
				.Take(DefaultPageSize)
				.ProjectTo<SongListingServiceModel>()
				.ToListAsync();
		}

		public async Task<int> TotalAsync()
		{
			return await this.context.Songs.CountAsync();
		}
	}
}
