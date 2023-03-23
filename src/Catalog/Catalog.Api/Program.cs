using Catalog.Api.Middlewares;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Configurations;

namespace Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<PersistenceConfiguration>(builder.Configuration.GetSection("Persistence"));

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}