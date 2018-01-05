namespace Mp3MusicZone.Web.Models.Shared
{
	using System;
	using System.Collections.Generic;

	public class PaginatedViewModel<TModel>
		where TModel : class
	{
		public IEnumerable<TModel> Items { get; set; }

		public int Current { get; set; }

		public int PageSize { get; set; }

		public int TotalPages { get; set; }

		public int Previous 
			=> this.Current == 1 ? 1 : this.Current - 1;

		public int Next 
			=> this.Current == this.TotalPages ? this.TotalPages : this.Current + 1;

	}
}
