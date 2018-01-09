namespace Mp3MusicZone.Web.Areas.Admin.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	using static Common.Constants.ModelConstants;

	public class CategoryFormModel
	{
		[Required]
		[MinLength(NameMinLength, ErrorMessage = MinLengthErrorMessage)]
		[MaxLength(NameMaxLength, ErrorMessage = MaxLengthErrorMessage)]
		public string Name { get; set; }
	}
}
