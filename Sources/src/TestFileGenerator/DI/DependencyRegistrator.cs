using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestFileGenerator.Configuration;
using TestFileGenerator.Generators;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Savers;

namespace TestFileGenerator.DI
{
    /// <summary>
    /// Registrator fo dependencies in the application.
    /// </summary>
    public static class DependencyRegistrator
    {
        /// <summary>
        /// Configure DI for Runner module components.
        /// </summary>
        /// <param name="serviceCollection">Services in container.</param>
        /// <param name="configuration">Configuration for application.</param>
        public static void InitializeDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // configure configurations
            serviceCollection.Configure<GeneratorConfiguration>(configuration.GetSection(nameof(GeneratorConfiguration)));
            
            // configure dependencies
            serviceCollection.AddSingleton<ITestFileGenerator, Generators.TestFileGenerator>();
            serviceCollection.AddSingleton<ITestDataGenerator, TestDataGenerator>();
            serviceCollection.AddSingleton<IFileSaver, CSVFileSaver>();
            serviceCollection.AddSingleton<IFilenameGenerator, FilenameGenerator>();
        }
    }
}