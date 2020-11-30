using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookie";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ClientId = "client_id_mvc";
                    options.ClientSecret = "client_secret_mvc";
                    options.ResponseType = "code";
                    options.SaveTokens = true;

                    // Configure cookie claim mapping
                    options.ClaimActions.DeleteClaim("amr");
                    options.ClaimActions.DeleteClaim("s_hash");
                    options.ClaimActions.MapUniqueJsonKey("MvcClient.Grandma", "rc.grandma");

                    // Two trips to load claims in to the cookie but the id token is smaller
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // Configure scope
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("rc.scope");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("ApiOne");
                    options.Scope.Add("ApiTwo");
                });

            services.AddHttpClient();

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
                app.UseExceptionHandler("/Home/Error");
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
