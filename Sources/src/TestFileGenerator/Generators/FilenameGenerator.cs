using System.IO;
using Microsoft.Extensions.Options;
using TestFileGenerator.Configuration;
using TestFileGenerator.Interfaces;

namespace TestFileGenerator.Generators
{
    // TODO: implement simple generator for file names 
    
    public sealed class FilenameGenerator : IFilenameGenerator
    {
        private readonly GeneratorConfiguration _generatorConfiguration;
        
        public FilenameGenerator(IOptionsSnapshot<GeneratorConfiguration> options)
        {
            _generatorConfiguration = options.Value;
        }

        public string GenerateCorrectFilename()
        {
            CheckFolder(_generatorConfiguration.OutputFolder);
            
            return Path.Combine( _generatorConfiguration.OutputFolder,"GeneratedFile_Correct.csv");
        }

        public string GenerateWrongFilename()
        {
            CheckFolder(_generatorConfiguration.OutputFolder);
            
            return Path.Combine(_generatorConfiguration.OutputFolder, "GeneratedFile_Wrong.csv");
        }

        private void CheckFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}