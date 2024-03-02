using Business;
using Chat_AspNetCore7.Hubs;
using Chat_AspNetCore7.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Serilog.Events;
using Serilog;

namespace Chat_AspNetCore7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.ConfigureServiceRegistration();
            builder.Services.ConfigureDbContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureSession();
            builder.Services.ConfigureRouting();
            builder.Services.AddHttpClient();
            builder.Services.ConfigureServiceRegistration();
            builder.Services.ConfigureRepositoryRegistration();
            builder.Services.ConfigureApplicationCookie();
            builder.Services.AddControllers();
            builder.Services.ConfigureMailSenderServices(builder.Configuration);
            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddSignalR();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            var app = builder.Build();

            app.ConfigureAndCheckMigration();
            app.ConfigureDefaultAdminUser();
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
                );

            app.MapControllerRoute("default", "{controller=Chat}/{action=Index}/{id?}");
            app.MapHub<ChatHub>("/chatHub");
            app.Run();
        }
    }
}

