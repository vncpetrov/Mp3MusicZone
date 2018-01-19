namespace Mp3MusicZone.Web.Components.Models
{
	using System;

	public class PaginationComponentViewModel
	{
		public string SearchTerm { get; set; }

		public int Current { get; set; }

		public int Previous { get; set; }

		public int Next { get; set; }

		public int StartPage { get; set; }

		public int EndPage { get; set; }

		public string PreviousDisabled { get; set; }

		public string NextDisabled { get; set; }
	}
}
