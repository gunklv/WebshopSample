using Testcontainers.MongoDb;

namespace Cart.ComponentTests.Infrastructure.TestContainerInitializers.MongoDb
{
    internal class MongoDbTestContainerInitializer
    {
        private const string ContainerName = "MongoDbContainer";
        private const string ImageName = "mongo";

        public static MongoDbTestContainer Init()
        {
            return new MongoDbTestContainer(
                new MongoDbBuilder()
                    .WithAutoRemove(true)
                    .WithImage(ImageName)
                    .WithName(ContainerName)
                    .WithCleanUp(true)
                    .Build());
        }
    }
}
