﻿using Cart.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Cart.ComponentTests.Infrastructure.TestContainerInitializers.MongoDb;
using Microsoft.Extensions.DependencyInjection;
using Cart.Api.Infrastructure.Configurations;

namespace Cart.ComponentTests.Infrastructure
{
    public class CartWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private MongoDbTestContainer MongoDbTestContainer { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var mongoPersistenceConfiguration = MongoDbTestContainer.GetConfiguration();
            builder.ConfigureServices(services => services.Configure<PersistenceConfiguration>(
                x =>
                {
                    x.ConnectionString = mongoPersistenceConfiguration.ConnectionString;
                    x.DatabaseName = mongoPersistenceConfiguration.DatabaseName;
                }));
        }

        public async Task InitializeAsync()
        {
            MongoDbTestContainer = MongoDbTestContainerInitializer.Init();
            await MongoDbTestContainer.StartAsync();
            await MongoDbTestContainer.GenerateDatabaseSchemaAsync();
        }

        public new async Task DisposeAsync()
        {
            await MongoDbTestContainer.DisposeAsync();
        }
    }
}
