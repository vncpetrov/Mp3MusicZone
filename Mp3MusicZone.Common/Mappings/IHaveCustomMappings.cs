namespace Mp3MusicZone.Common.Mappings
{
	using AutoMapper;
	using System;

	public interface IHaveCustomMappings
    {
		void ConfigureMappings(Profile config);
    }
}
