namespace Mp3MusicZone.Web.Areas.Uploader.Models
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using static Common.Constants.ModelConstants;

	public class SongFormModel
	{
		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; }

		[Required]
		[MinLength(NameMinLength)]
		[MaxLength(NameMaxLength)]
		public string Singer { get; set; }

		[Required(ErrorMessage = "Please, choose a file!")]
		public IFormFile Song { get; set; }

		[Display(Name = "Category")]
		public int CategoryId { get; set; }

		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}
