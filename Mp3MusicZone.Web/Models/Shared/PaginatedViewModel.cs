namespace Mp3MusicZone.Web.Models.Shared
{
	using System;
	using System.Collections.Generic;

	public class PaginatedViewModel<TModel> : IPagination
		where TModel : class
	{
		public IEnumerable<TModel> Items { get; set; }

		public int Current { get; set; }

		public int PageSize { get; set; }

		public int TotalItems { get; set; }
	}
}
