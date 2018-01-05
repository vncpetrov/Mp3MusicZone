namespace Mp3MusicZone.Common.Attributes.Validation
{
	using Providers;
	using Providers.Contracts;
	using System;

	public class MinAgeAttribute : PropertyValidationAttribute
	{
		private readonly int minimumAge;
		private IDateTimeProvider dateTimeProvider;

		public MinAgeAttribute(int minimumAge)
		{
			this.minimumAge = minimumAge;
		}

		public IDateTimeProvider DateTimeService
		{
			get
			{
				if (this.dateTimeProvider is null)
				{
					this.dateTimeProvider = new DateTimeProvider();
				}

				return this.dateTimeProvider;
			}
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				this.dateTimeProvider = value;
			}
		}

		public override bool IsValid(object value)
		{
			DateTime? valueAsDateTime = value as DateTime?;

			if (valueAsDateTime is null)
			{
				return true;
			}

			DateTime birthdate = valueAsDateTime.Value;
			DateTime currentDate = this.DateTimeService.UtcNow;

			int age = currentDate.Year - birthdate.Year;

			if (birthdate > currentDate.AddYears(-age))
			{
				age--;
			}

			return age >= minimumAge;
		}
	}
}
