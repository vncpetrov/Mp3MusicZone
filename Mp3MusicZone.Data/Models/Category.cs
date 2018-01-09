namespace Mp3MusicZone.Data.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using static Common.Constants.ModelConstants;

	public class Category
	{
		public int Id { get; set; }

		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
	}
}
