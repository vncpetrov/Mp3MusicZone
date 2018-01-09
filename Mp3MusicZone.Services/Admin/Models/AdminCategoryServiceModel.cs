namespace Mp3MusicZone.Services.Admin.Models
{
	using Common.Mappings;
	using Data.Models;
	using System;

	public class AdminCategoryServiceModel : IMapFrom<Category>
    {
		public string Name { get; set; }
	}
}
