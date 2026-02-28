using GalleryData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GalleryData.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using MadlenArtGallery.Areas.Identity.Data;
using MadlenArtGallery.Services;
using MadlenArtGallery.Services.Interfaces;

namespace MadlenArtGallery
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddHostedService<AuctionService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IArtworkService, ArtworkService>();
            builder.Services.AddScoped<IBidService, BidService>();
            builder.Services.AddScoped<IFrameService, FrameService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            var app = builder.Build();

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<MadlenArtGallery.Hubs.BidHub>("/bidHub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    await GalleryDataSeeder.Seed(services);
            //}
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeeder.SeedRolesAndAdmin(services);
            }           
            app.Run();
        }
    }
}
