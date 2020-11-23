using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication
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
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    // Default "/Account/Login"
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(authorizationOptions =>
            {
                // var defaultAuthPolicyBuilder = new AuthorizationPolicyBuilder();
                // var defaultAuthPolicy = defaultAuthPolicyBuilder
                //     .RequireAuthenticatedUser()
                //     .RequireClaim(ClaimTypes.DateOfBirth)
                //     .Build();

                // authorizationOptions.DefaultPolicy = defaultAuthPolicy;


                // authorizationOptions.AddPolicy("Claim.DoB", policyBuilder =>
                // {
                //     policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                // });


                // authorizationOptions.AddPolicy("Claim.DoB", policyBuilder =>
                // {
                //     policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                // });


                // With extension
                authorizationOptions.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });

                authorizationOptions.AddPolicy("Admin", policyBuilder =>
                    policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

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

            // Who are you?
            app.UseAuthentication();
            // Are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
