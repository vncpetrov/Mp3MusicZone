namespace Mp3MusicZone.Data.Models
{
	using Common.Attributes.Validation;
	using Enums;
	using Microsoft.AspNetCore.Identity;
	using System;
	using System.ComponentModel.DataAnnotations;

	using static Mp3MusicZone.Common.Constants.ModelConstants;

	public class User : IdentityUser
	{
		[Required]
		[DataType(DataType.Date)]
		[MinAge(16)]
		public DateTime Birthdate { get; set; }

		public GenreType Genre { get; set; }

		[Required]
		[NumberRange(NameMinLength, NameMaxLength)]
		public string FirstName { get; set; }

		[Required]
		[NumberRange(NameMinLength, NameMaxLength)]
		public string LastName { get; set; }
	}
}
