namespace Mp3MusicZone.Web.Models.Shared
{
	using System;

	public interface IPagination
    {
		int Current { get; set; }

		int PageSize { get; set; }

		int TotalPages { get; set; }

		int Previous { get; }

		int Next { get; }
	}
}
