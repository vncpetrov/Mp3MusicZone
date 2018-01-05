namespace Mp3MusicZone.Services.Admin.Models
{
	using Common.Mappings;
	using Data.Models;
	using System;
	using System.Collections.Generic;

	public class AdminUserListingServiceModel : IMapFrom<User>
	{
		public string Id { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public IEnumerable<string> Roles { get; set; }
	}
}
