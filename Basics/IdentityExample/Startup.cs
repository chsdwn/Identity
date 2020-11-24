using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace IdentityExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseInMemoryDatabase("MemoryDB");
            });

            // AddIdentity registers the services
            services.AddIdentity<IdentityUser, IdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 4;
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(cookieAuthenticationOptions =>
            {
                cookieAuthenticationOptions.Cookie.Name = "Identity.Cookie";
                cookieAuthenticationOptions.LoginPath = "/Home/Login";
            });

            // services.AddAuthentication("CookieAuth")
            //     .AddCookie("CookieAuth", config =>
            //     {
            //         config.Cookie.Name = "Grandmas.Cookie";
            //         // Default "/Account/Login"
            //         config.LoginPath = "/Home/Authenticate";
            //     });

            services.AddMailKit(mailKitOptionsBuilder =>
                mailKitOptionsBuilder.UseMailKit(Configuration.GetSection("Email").Get<MailKitOptions>())
            );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
