namespace Mp3MusicZone.Services.Models
{
	using Mp3MusicZone.Common.Mappings;
	using Mp3MusicZone.Data.Models;
	using System;

	public class SongListingServiceModel : IMapFrom<Song>
    {
		public string Name { get; set; }
		
		public string Singer { get; set; }
	}
}
