using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.Services;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using ShoppingWebsite.Models;
using ShoppingWebsite.Hubs;

namespace ShoppingWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            var services = builder.Services;

            services.AddControllers();
            builder.Services.AddRazorPages();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddSession();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.Cookie.MaxAge = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
            });

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
                        .UseLazyLoadingProxies();
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "cookie";
                options.DefaultScheme = "cookie";
            })
            .AddCookie("cookie", options =>
            {
                options.LoginPath = "/";
                options.AccessDeniedPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
            })
            .AddGoogle(options =>
            {
                var googleConfig = builder.Configuration.GetSection("ExtenalLogin:Google");
                options.ClientId = "852553651258-jbfsa4o2i84apok6ssaitim4kpq9rb06.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireAssertion(context =>
                {
                    var role = context.User.FindFirstValue(ClaimTypes.Role);
                    return role == "Staff";
                }));
            });

            services.AddScoped<ICookieAuthentication, CookieAuthentication>();

            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHub<HubServer>("/noti");
            app.MapHub<CallHubServer>("/callserver");

            app.UseEndpoints(ep =>
            {
                ep.MapRazorPages();
                ep.MapControllers();
            });

            app.Run();
        }
    }
}