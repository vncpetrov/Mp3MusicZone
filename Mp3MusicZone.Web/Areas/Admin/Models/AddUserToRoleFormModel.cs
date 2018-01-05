namespace Mp3MusicZone.Web.Areas.Admin.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class UserRoleFormModel
    {
		[Required]
		public string UserId { get; set; }

		[Required]
		public string Role { get; set; }
	}
}
