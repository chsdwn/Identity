using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Server
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
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", jwtBearerOptions =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);

                    // Catches Bearer token sends from url.
                    // localhost:5000/home/secret?access_token=token
                    jwtBearerOptions.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = messageReceivedContext =>
                        {
                            if (messageReceivedContext.Request.Query.ContainsKey("access_token"))
                                messageReceivedContext.Token = messageReceivedContext.Request.Query["access_token"];


                            return Task.CompletedTask;
                        }
                    };

                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Constants.Issuer,
                        ValidAudience = Constants.Audiance,
                        IssuerSigningKey = key
                    };
                });

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
