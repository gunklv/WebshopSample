using Cart.Api.Api.Mappers;
using Cart.Api.Api.Mappers.Abstractions;
using Cart.Api.Core.Abstractions.Repositories;
using Cart.Api.Core.Services;
using Cart.Api.Core.Services.Abstractions;
using Cart.Api.Infrastructure.Configurations;
using Cart.Api.Infrastructure.Repositories;

namespace Cart.Api
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

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}