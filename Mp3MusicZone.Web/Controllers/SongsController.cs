namespace Mp3MusicZone.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Models.Shared;
	using Mp3MusicZone.Services.Models;
	using Services.Contracts;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;

	using static Common.Constants.ServiceConstants;
	using static Common.Constants.WebConstants;


	public class SongsController : Controller
	{
		private readonly ISongService songService;

		public SongsController(ISongService songService)
		{
			this.songService = songService;
		}

		public async Task<IActionResult> All(int page = 1)
		{
			int totalSongs = await this.songService.TotalAsync();
			int pageSize = DefaultPageSize;
			int totalPages = (int)Math.Ceiling((double)totalSongs / pageSize);

			if (page < 1)
			{
				page = 1;
			}

			if (totalPages > 0 && page > totalPages)
			{
				page = totalPages;
			}

			IEnumerable<SongListingServiceModel> songs =
				await this.songService.GetAllAsync(page);

			return View(new PaginatedViewModel<SongListingServiceModel>()
			{
				Current = page,
				PageSize = pageSize,
				TotalPages = totalPages,
				Items = songs
			});
		}

		[HttpGet]
		public async Task<IActionResult> Play(string songName)
		{
			string fileWithExtension = $"{songName}.{SongExtension}";
			string filePath = $"{DirectoryPath}/{fileWithExtension}";

			using (MemoryStream memoryStream = new MemoryStream())
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
			{
				await fileStream.CopyToAsync(memoryStream);

				await this.songService.IncrementListeningsAsync(songName);

				this.HttpContext.Response.Headers.Add("Accept-Ranges", "bytes");

				return File(memoryStream.ToArray(), "audio/mp3", fileWithExtension);
			}
		}
	}
}
