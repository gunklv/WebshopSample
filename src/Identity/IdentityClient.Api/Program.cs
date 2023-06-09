using IdentityClient.Api.Pages.Account;
using IdentityClient.Api.Pages.Catalog;

namespace MvcClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<CatalogConfiguration>(builder.Configuration.GetSection("ApiConfiguration:CatalogConfiguration"));
            builder.Services.Configure<ProfileConfiguration>(x => x.BaseUrl = builder.Configuration.GetValue<string>("IdentityConfiguration:Authority"));

            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = builder.Configuration.GetValue<string>("IdentityConfiguration:Authority");

                    options.ClientId = builder.Configuration.GetValue<string>("IdentityConfiguration:ClientId");
                    options.ClientSecret = builder.Configuration.GetValue<string>("IdentityConfiguration:ClientSecret");
                    options.ResponseType = builder.Configuration.GetValue<string>("IdentityConfiguration:ResponseType");

                    options.SaveTokens = true;

                    options.Scope.Clear();
                    options.Scope.Add("offline_access");
                    options.Scope.Add("openid");
                    options.Scope.Add("name");
                    options.Scope.Add("roles");
                    options.Scope.Add("role");

                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            var app = builder.Build();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

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

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}