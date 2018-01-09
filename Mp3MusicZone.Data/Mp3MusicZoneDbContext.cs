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

		public DbSet<Song> Songs { get; set; }

		public DbSet<Category> Categories { get; set; }
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Song>()
				.HasOne(s => s.Category)
				.WithMany(c => c.Songs)
				.HasForeignKey(s => s.CategoryId);

			builder.Entity<User>()
				.HasMany(u => u.Songs)
				.WithOne(s => s.Uploader)
				.HasForeignKey(s => s.UploaderId);

			base.OnModelCreating(builder);
		}
	}
}
