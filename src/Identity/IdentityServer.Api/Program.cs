using IdentityServer.Api.Configuration;
using IdentityServer.Api.Services;
using IdentityServer.Contexts;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddRazorPages();

    var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
    var connectionString = builder.Configuration.GetValue<string>("AccountConfiguration:ConnectionString");

    builder.Services.AddDbContext<AccountDbContext>(options => options.UseNpgsql(connectionString));

    builder.Services
        .AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AccountDbContext>()
        .AddDefaultTokenProviders();

    var identityConfiguration = new IdentityConfiguration();
    builder.Configuration.GetSection("IdentityConfiguration").Bind(identityConfiguration);

    builder.Services
        .AddIdentityServer()
        .AddInMemoryIdentityResources(InMemoryConfig.IdentityResources)
        .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
        .AddInMemoryClients(InMemoryConfig.Clients(identityConfiguration))
        .AddInMemoryApiResources(InMemoryConfig.ApiResources)
        .AddProfileService<ProfileService>()
        .AddAspNetIdentity<ApplicationUser>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseStaticFiles();
    app.UseRouting();

    app.UseIdentityServer();

    app.UseAuthorization();
    app.MapRazorPages().RequireAuthorization();


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}