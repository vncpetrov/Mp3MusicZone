namespace Mp3MusicZone.Web.Models.Account
{
	using Data.Models.Enums;
	using System;
	using System.ComponentModel.DataAnnotations;

	using static Mp3MusicZone.Common.Constants.ModelConstants;

	public class RegisterViewModel
	{
		[Required]
		[MinLength(UsernameMinLength,
			ErrorMessage = MinLengthErrorMessage)]
		[MaxLength(UsernameMaxLength,
			ErrorMessage = MaxLengthErrorMessage)]
		public string Username { get; set; }

		[Required]
		[MinLength(PasswordMinLength,
			ErrorMessage = MinLengthErrorMessage)]
		[MaxLength(PasswordMaxLength,
			ErrorMessage = MaxLengthErrorMessage)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare(nameof(Password),
			ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "E-mail")]
		public string Email { get; set; }

		[Required]
		[MinLength(NameMinLength,
			ErrorMessage = MinLengthErrorMessage)]
		[MaxLength(NameMaxLength,
			ErrorMessage = MaxLengthErrorMessage)]
		public string FirstName { get; set; }

		[Required]
		[MinLength(NameMinLength, 
			ErrorMessage = MinLengthErrorMessage)]
		[MaxLength(NameMaxLength,
			ErrorMessage = MaxLengthErrorMessage)]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Birthdate { get; set; }

		[Required]
		public GenreType Genre { get; set; }
	}
}
