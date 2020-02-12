using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using TestFileGenerator.DI;

namespace TestFileGenerator
{
    static class Program
    {
        /// <summary>
        /// Prefix for all environment variables.
        /// </summary>
        private const string Prefix = "TFG_"; // TestFileGenerator

        /// <summary>
        /// Default file with settings for application.
        /// </summary>
        private const string AppSettings = "appsettings.json";

        /// <summary>
        /// Host settings.
        /// </summary>
        private const string HostSettings = "hostsettings.json";

        /// <summary>
        /// Prefix for file with application settings for different environments.
        /// </summary>
        private const string AppSettingsPrefix = "appsettings";

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static async Task<int> Main(string[] args)
        {
            // Get current directory.
            var appLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // build and run Host
            return await CreateHostBuilder(appLocation)
                .RunCommandLineApplicationAsync<Application>(args);
        }

        /// <summary>
        /// Create and configure host with settings and other things.
        /// </summary>
        /// <returns>Returns configured host builder to build and run.</returns>
        private static IHostBuilder CreateHostBuilder(string appLocation) =>
            new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(appLocation);
                    configHost.AddJsonFile(HostSettings, true);
                    configHost.AddEnvironmentVariables(Prefix);
                }).ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(appLocation);
                    configApp.AddJsonFile(AppSettings, optional: true);
                    configApp.AddJsonFile($"{AppSettingsPrefix}.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: Prefix);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.InitializeDependencies(hostContext.Configuration);
                })
                .UseSerilog((hostContext, configLog) =>
                {
                    configLog.ReadFrom.Configuration(hostContext.Configuration);
                });
    }
}