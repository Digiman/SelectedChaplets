using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using TestFileGenerator.Interfaces;

namespace TestFileGenerator.Commands
{
    /// <summary>
    /// Command to call logic to generate the files based on the task data from the Lab 1.
    /// </summary>
    [Command("generate", Description = "Generate test files based on parameters and save to output folder.", ShowInHelpText = true)]
    public sealed class GenerateFileCommand : ICustomCommand
    {
        private readonly ILogger<GenerateFileCommand> _logger;
        private readonly ITestFileGenerator _testFileGenerator;

        [Option(CommandOptionType.SingleValue, Description = "Students count", ShortName = "s")]
        public int StudentsCount { get; }
        
        [Option(CommandOptionType.SingleValue, Description = "Subjects count", ShortName = "sb")]
        public int SubjectsCount { get; }
        
        public GenerateFileCommand(ILogger<GenerateFileCommand> logger, ITestFileGenerator testFileGenerator)
        {
            _logger = logger;
            _testFileGenerator = testFileGenerator;
        }

        public async Task OnExecuteAsync(CommandLineApplication command, IConsole console)
        {
            _logger.LogInformation("Generate test file with sample data");
            
            try
            {
                // generate sample correct file
                await _testFileGenerator.GenerateCorrectFileAsync(StudentsCount, SubjectsCount);

                // generate sample wrong file
                await _testFileGenerator.GenerateWrongFileAsync(StudentsCount, SubjectsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during generation of the test file!", ex);

                throw;
            }
        }
    }
}