namespace Mp3MusicZone.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Models;
	using System;

	public class Mp3MusicZoneDbContext : IdentityDbContext<User>
	{
		public Mp3MusicZoneDbContext(DbContextOptions<Mp3MusicZoneDbContext> options)
			:base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
