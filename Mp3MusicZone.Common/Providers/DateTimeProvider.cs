namespace Mp3MusicZone.Common.Providers
{
	using Contracts;
	using System;

	public class DateTimeProvider : IDateTimeProvider
	{
		public virtual DateTime UtcNow => DateTime.UtcNow;
	}
}
