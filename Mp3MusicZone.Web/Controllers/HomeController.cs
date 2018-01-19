namespace Mp3MusicZone.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Models;
	using Services.Contracts;
	using Services.Models;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Web.Models.Shared;

	using static Common.Constants.WebConstants;

	public class HomeController : Controller
	{
		private readonly ISongService songService;

		public HomeController(ISongService songService)
		{
			this.songService = songService;
		}

		public async Task<IActionResult> Index(int page = 1, string searchTerm = null)
		{
			int totalSongs = await this.songService.TotalAsync(searchTerm);
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
				await this.songService.GetAllAsync(page, searchTerm);

			return View(new PaginatedSearchViewModel<SongListingServiceModel>()
			{
				SearchTerm = searchTerm,
				PageInfo = new PaginatedViewModel<SongListingServiceModel>()
				{
					Current = page,
					PageSize = pageSize,
					TotalPages = totalPages,
					Items = songs
				}
			});
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
