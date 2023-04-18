using Catalog.Api.Middlewares;
using Catalog.Api.Utilities.Hateoas;
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

            builder.Services.AddControllers(options => {
                options.OutputFormatters.Add(new HalJsonMediaTypeFormatter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}