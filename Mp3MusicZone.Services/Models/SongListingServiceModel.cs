namespace Mp3MusicZone.Services.Models
{
	using Mp3MusicZone.Common.Mappings;
	using Mp3MusicZone.Data.Models;
	using System;
	using AutoMapper;

	public class SongListingServiceModel : IMapFrom<Song>,IHaveCustomMappings
    {
		public string Name { get; set; }
		
		public string Singer { get; set; }

		public string UploaderUsername { get; set; }
		public string UploaderId { get; set; }

		public int Listenings { get; set; }

		public void ConfigureMappings(Profile config)
		{
			config.CreateMap<Song, SongListingServiceModel>()
				.ForMember(m => m.UploaderUsername,
					cfg => cfg.MapFrom(s => s.Uploader.UserName));
		}
	}
}
