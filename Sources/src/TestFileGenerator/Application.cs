﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestFileGenerator.Commands;

namespace TestFileGenerator
{
    /// <summary>
    /// Entry point in application to run default command and also configure sub commands in the app.
    /// </summary>
    [Subcommand(typeof(GenerateFileCommand))]
    public sealed class Application
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private readonly ILogger<Application> _logger;

        /// <summary>
        /// Hosting environment provider.
        /// </summary>
        private readonly IHostEnvironment _hostEnvironment;

        public Application(ILogger<Application> logger, IHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostingEnvironment;
        }
        
        /// <summary>
        /// Default action after running the application root.
        /// </summary>
        /// <param name="application">Command line application to run and configure.</param>
        /// <param name="console">Wrapper to use console.</param>
        private void OnExecute(CommandLineApplication application, IConsole console)
        {
            var message = $"Environment: {_hostEnvironment.EnvironmentName}";

            console.WriteLine("TestFileGenerator Tool");
            console.WriteLine(message);

            application.ShowHelp();
        }
    }
}