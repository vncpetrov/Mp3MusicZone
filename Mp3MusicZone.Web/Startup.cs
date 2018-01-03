namespace Mp3MusicZone.Web
{
	using System;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Mp3MusicZone.Data.Models;
	using Mp3MusicZone.Data;
	using Microsoft.AspNetCore.Mvc;
	using Mp3MusicZone.Services.Contracts;
	using Mp3MusicZone.Services;
	using Mp3MusicZone.Web.Infrastructure.Extensions;
	using AutoMapper;
	using Mp3MusicZone.Services.Models;

	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
		
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Mp3MusicZoneDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;

				options.SignIn.RequireConfirmedEmail = true;
			})
                .AddEntityFrameworkStores<Mp3MusicZoneDbContext>()
                .AddDefaultTokenProviders();

			services.AddServices();
			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

			services.AddAutoMapper();

            services.AddMvc(options =>
			{
				options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
			});
        }
		
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
