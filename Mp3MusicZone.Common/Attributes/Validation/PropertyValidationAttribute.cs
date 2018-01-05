namespace Mp3MusicZone.Common.Attributes.Validation
{
	using System;
	using System.ComponentModel.DataAnnotations;

	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public abstract class PropertyValidationAttribute : ValidationAttribute
	{
	}
}
