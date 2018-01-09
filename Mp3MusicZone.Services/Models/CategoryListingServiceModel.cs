namespace Mp3MusicZone.Services.Models
{
	using Common.Mappings;
	using Data.Models;
	using System;

	public class CategoryListingServiceModel : IMapFrom<Category>
    {
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
