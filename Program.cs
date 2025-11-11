using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;

namespace MunicipalServicesApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                    ?? "Data Source=municipalservices.db"));


            builder.Services.AddScoped<MunicipalServicesApp.Services.IReportIssueService, MunicipalServicesApp.Services.ReportIssueService>();
            builder.Services.AddScoped<MunicipalServicesApp.Services.IEventService, MunicipalServicesApp.Services.EventService>();
            builder.Services.AddScoped<MunicipalServicesApp.Services.IAdminAuthService, MunicipalServicesApp.Services.AdminAuthService>();
            builder.Services.AddScoped<MunicipalServicesApp.Services.IEventRsvpService, MunicipalServicesApp.Services.EventRsvpService>();

            // Session support for admin authentication
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Db is created and seed initial data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                DbInitializer.SeedData(context);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
               // https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
