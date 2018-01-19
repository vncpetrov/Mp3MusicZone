namespace Mp3MusicZone.Web.Components
{
	using Microsoft.AspNetCore.Mvc;
	using System;
	using Web.Components.Models;
	using Web.Models.Shared;

	public class PaginationComponent : ViewComponent
	{
		public IViewComponentResult Invoke(IPagination pageInfo, string searchTerm)
		{
			int startPage = pageInfo.Current - 1 <= 0
				? 1
				: pageInfo.Current - 1;

			int endPage = pageInfo.Current + 1 >= pageInfo.TotalPages
				? pageInfo.TotalPages
				: pageInfo.Current + 1;

			if (startPage == 1)
			{
				endPage = pageInfo.TotalPages >= 3 ? 3 : pageInfo.TotalPages;
			}

			if (endPage == pageInfo.TotalPages && pageInfo.Current == endPage)
			{
				startPage = startPage == 1 ? 1 : startPage - 1;
			}

			string previousDisabled =
				pageInfo.Current <= 1 ? "disabled" : string.Empty;
			string nextDisabled =
				pageInfo.Current >= pageInfo.TotalPages ? "disabled" : string.Empty;

			PaginationComponentViewModel model = new PaginationComponentViewModel()
			{
				SearchTerm = searchTerm,
				Current = pageInfo.Current,
				Previous = pageInfo.Previous,
				Next = pageInfo.Next,
				StartPage = startPage,
				EndPage = endPage,
				PreviousDisabled = previousDisabled,
				NextDisabled = nextDisabled
			};

			return View("_Pagination", model);
		}
	}
}
