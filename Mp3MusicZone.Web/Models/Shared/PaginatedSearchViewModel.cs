namespace Mp3MusicZone.Web.Models.Shared
{
	using System;

	public class PaginatedSearchViewModel<TModel>
		where TModel:class
    {
		public string SearchTerm { get; set; }

		public PaginatedViewModel<TModel> PageInfo { get; set; }
	}
}
