﻿namespace Mp3MusicZone.Data.Models
{
	using Common.Attributes.Validation;
	using Enums;
	using Microsoft.AspNetCore.Identity;
	using System;
	using System.Collections.Generic;
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
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string FirstName { get; set; }

		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string LastName { get; set; }

		public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
	}
}
