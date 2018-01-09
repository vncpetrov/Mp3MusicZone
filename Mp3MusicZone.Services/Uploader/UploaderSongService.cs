namespace Mp3MusicZone.Services.Uploader
{
	using Contracts;
	using Data;
	using Microsoft.EntityFrameworkCore;
	using Mp3MusicZone.Data.Models;
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using static Common.Constants.ServiceConstants;

	public class UploaderSongService : IUploaderSongService
	{
		private readonly Mp3MusicZoneDbContext context;

		public UploaderSongService(Mp3MusicZoneDbContext context)
		{
			this.context = context;
		}

		public async Task<bool> ExistsAsync(string name)
			=> await this.context.Songs
				.AnyAsync(s => s.Name == name);


		public async Task<bool> UploadAsync(
			string userId,
			string name,
			string singer,
			int categoryId,
			byte[] file)
		{
			if (!await this.context.Users.AnyAsync(u => u.Id == userId))
			{
				return false;
			}

			if (!Directory.Exists(DirectoryPath))
			{
				Directory.CreateDirectory(DirectoryPath);
			}

			string filePath = $"{DirectoryPath}/{name}.{SongExtension}";

			File.WriteAllBytes(filePath, file);

			Song song = new Song()
			{
				Name = name,
				Singer = singer,
				UploaderId = userId,
				CategoryId = categoryId
			};

			await this.context.Songs.AddAsync(song);
			await this.context.SaveChangesAsync();

			return true;
		}
	}
}
