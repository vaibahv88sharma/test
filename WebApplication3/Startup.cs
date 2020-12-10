using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class AzureAd
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string CallbackPath { get; set; }
    }
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
            var cipAzureAd = Configuration.GetSection(nameof(AzureAd)).Get<AzureAd>();

            /*
                 "Instance": "https://login.microsoftonline.com/",
                "Domain": "vaibhav88sharmahotmail.onmicrosoft.com",
                "TenantId": "8a6f944c-79e3-42cc-98c3-1ac6cf5edfd8",  Directory (tenant) ID
                "ClientId": "e17a3e26-63dc-4221-970a-44ef2b631027", Application (client)
                "CallbackPath": "/signin-oidc"
             */

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options =>
            {
                //options.Authority = "https://login.microsoftonline.com/YOUR_TENANT_ID";
                options.Authority = $"{cipAzureAd.Instance}{cipAzureAd.TenantId}";
                //options.ClientId = "YOUR_APPLICATION_ID";
                options.ClientId = $"{cipAzureAd.ClientId}";
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                //options.CallbackPath = "/auth/signin-callback";
                options.CallbackPath = $"{cipAzureAd.CallbackPath}";
                //options.SignedOutRedirectUri = "https://localhost:44379/";
                options.SignedOutRedirectUri = "https://localhost:44376/";
                options.TokenValidationParameters.NameClaimType = "name";
            })
            //.AddCookie();
            .AddCookie(config =>
            {
                //config.Cookie.Name = "UserLoginCookie";
                //config.LoginPath = "/Login/UserLogin";
                config.LoginPath = "/auth/login";
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
