using IdentityServer;
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
        const string connectionString = @"Server=localhost:5433; Username=admin; Password=admin; Database=identity";

        builder.Services.AddDbContext<AccountDbContext>(options => options.UseNpgsql(connectionString));

        builder.Services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AccountDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiResources(Config.ApiResources)
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