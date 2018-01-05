namespace Mp3MusicZone.Common.Providers.Contracts
{
	using System;

	public interface IDateTimeProvider
    {
		DateTime UtcNow { get; }
    }
}
