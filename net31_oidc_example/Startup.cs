using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;


namespace net31_oidc_example
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
            services.AddRazorPages();

            services.AddAuthentication(x => {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(x => {
                x.Cookie.HttpOnly = true;
                x.ExpireTimeSpan = TimeSpan.FromHours(2);
            })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = "{client_id}";
                options.ClientSecret = "{client_secret}";
                options.Authority = "{tenant_id}/oidc/endpoint/default";
                options.SignInScheme = "Cookies";
                options.ResponseType = "code";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });

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
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication(); //<- Add this line
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages(); //<-- Commentt this out
                endpoints.MapRazorPages().RequireAuthorization(); //<-- Add this line
            });
        }
    }
}
