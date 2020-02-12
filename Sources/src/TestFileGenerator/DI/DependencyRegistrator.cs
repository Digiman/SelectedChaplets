using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestFileGenerator.Generators;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Savers;

namespace TestFileGenerator.DI
{
    public static class DependencyRegistrator
    {
        /// <summary>
        /// Configure DI for Runner module components.
        /// </summary>
        /// <param name="serviceCollection">Services in container.</param>
        /// <param name="configuration"></param>
        public static void InitializeDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<ITestFileGenerator, Generators.TestFileGenerator>();
            serviceCollection.AddSingleton<ITestDataGenerator, TestDataGenerator>();
            serviceCollection.AddSingleton<IFileSaver, CSVFileSaver>();
        }
    }
}