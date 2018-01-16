namespace Mp3MusicZone.Data.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using static Common.Constants.ModelConstants;

	public class Song
    {
		public int Id { get; set; }

		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Singer { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }

		public string UploaderId { get; set; }
		public User Uploader { get; set; }

		public int Listenings { get; set; }
	}
}
