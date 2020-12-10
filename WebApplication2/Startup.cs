using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
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

namespace WebApplication2
{
    public class AzureAd {
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

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options => {
            //    options.Cookie.Name = "CIPPortal";
            //            })
            //.AddOpenIdConnect(options => ConfigOpenId(options, AzureAd));


            services.AddControllersWithViews();
        }

        //private OpenIdConnectOptions ConfigOpenId(OpenIdConnectOptions options, AzureAd cipAzureAd)
        //{

        //    options.Authority = $"{cipAzureAd.IdaAADInstance}{cipAzureAd.IdaTenantId}";
        //    options.ClientId = $"{cipAzureAd.IdaClientId}";
        //    options.ResponseType = OpenIdConnectResponseType.IdToken;
        //    options.CallbackPath = "/auth/signin-callback";
        //    options.SignedOutRedirectUri = $"{cipAzureAd.IdaPostLogoutRedirectUri}";
        //    options.TokenValidationParameters.NameClaimType = "name";

        //    //options. // CIPPortal
        //    return options;
        //}

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

            app.UseAuthentication(); // UseAuthentication must come before UseAuthorization
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
