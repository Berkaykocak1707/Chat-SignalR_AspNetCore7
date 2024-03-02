using DataAccess;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat_AspNetCore7.Infrastructure.Extensions
{
	public static class ApplicationExtension
	{
		public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
		{
			RepositoryContext context = app
				.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<RepositoryContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}
		}
		public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
		{
			const string adminUser = "Admin";
			const string adminPassword = "Admin17";

			//UserManager
			UserManager<CustomUser> userManager = app
				.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<UserManager<CustomUser>>();
			//RoleManager
			RoleManager<IdentityRole> roleManager = app
				.ApplicationServices
				.CreateAsyncScope()
				.ServiceProvider
				.GetRequiredService<RoleManager<IdentityRole>>();

			CustomUser user = await userManager.FindByNameAsync(adminUser);
			if (user == null)
			{
				user = new CustomUser()
				{
					EmailConfirmed = true,
					FirstName = "Berkay",
					LastName = "Kocak",
					Email = "berkaykocak1707@gmail.com",
					PhoneNumber = "5319576193",
					UserName = adminUser,
					UserPhotoUrl = "/images/no_photo.jpg"
                };
				var result = await userManager.CreateAsync(user, adminPassword);
				if (!result.Succeeded)
				{
					throw new Exception("Admin user could not created.");
				}

                var roles = roleManager.Roles.Where(r => r.Name != null).Select(r => r.Name!).ToList();
                var roleResult = await userManager.AddToRolesAsync(user, roles);
                if (!roleResult.Succeeded)
				{
					throw new Exception("Roller atanamadı.");
				}
			}
		}
		public static void ConfigureLocalization(this WebApplication web)
		{
			web.UseRequestLocalization(options =>
			{
				options.AddSupportedCultures("en-US")
				.AddSupportedCultures("en-US")
				.SetDefaultCulture("en-US");
			});
		}
	}
}
