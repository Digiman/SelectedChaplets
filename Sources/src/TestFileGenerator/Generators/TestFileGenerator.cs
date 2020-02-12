using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestFileGenerator.Configuration;
using TestFileGenerator.Enums;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Models;

namespace TestFileGenerator.Generators
{
    /// <summary>
    /// Sample implementation of the logic to generate test files for Lab 1 to test the solution from te students.
    /// </summary>
    public sealed class TestFileGenerator : ITestFileGenerator
    {
        private readonly ILogger<TestFileGenerator> _logger;
        private readonly ITestDataGenerator _testDataGenerator;
        private readonly IFileSaver _fileSaver;
        private readonly IFilenameGenerator _filenameGenerator;
        private readonly GeneratorConfiguration _generatorConfiguration;
        
        public TestFileGenerator(ILogger<TestFileGenerator> logger, ITestDataGenerator testDataGenerator,
            IFileSaver fileSaver, IFilenameGenerator filenameGenerator, IOptionsSnapshot<GeneratorConfiguration> options)
        {
            _logger = logger;
            _testDataGenerator = testDataGenerator;
            _fileSaver = fileSaver;
            _filenameGenerator = filenameGenerator;
            _generatorConfiguration = options.Value;
        }

        public async Task GenerateCorrectFileAsync(int students, int subjects, Language language)
        {
            try
            {
                int studentsCount = 0;
                int subjectsCount = 0;

                studentsCount = students == 0 ? _generatorConfiguration.DefaultStudentsCount : students;
                subjectsCount = subjects == 0 ? _generatorConfiguration.DefaultSubjectsCount : subjects;

                // 1. Generate the data.
                var group = await _testDataGenerator.GenerateDataAsync(studentsCount, subjectsCount, language);

                // 2. Save to the file.
                var filename = _filenameGenerator.GenerateCorrectFilename();

                _fileSaver.SaveDataToFile(filename, group, language);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during generation of the file!", ex);
                
                throw;
            }
        }

        public async Task GenerateWrongFileAsync(int students, int subjects, Language language)
        {
            try
            {
                int studentsCount = 0;
                int subjectsCount = 0;

                studentsCount = students == 0 ? _generatorConfiguration.DefaultStudentsCount : students;
                subjectsCount = subjects == 0 ? _generatorConfiguration.DefaultSubjectsCount : subjects;

                // 1. Generate the data.
                var group = await _testDataGenerator.GenerateDataAsync(studentsCount, subjectsCount, language);

                // 2. Modify some data to have wrong file as result.
                ModifyGroup(group);

                // 3. Save to the file.
                var filename = _filenameGenerator.GenerateWrongFilename();

                _fileSaver.SaveDataToFile(filename, group, language);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during generation of the file!", ex);

                throw;
            }
        }

        #region Helpers.
        
        private void ModifyGroup(Group group)
        {
            // remove 1 subject from the header
            var tmp = group.Subjects;
            group.Subjects = new List<string>();
            for (var i = 0; i < tmp.Count - 1; i++)
            {
                group.Subjects.Add(tmp[i]);
            }
        }
        
        #endregion
    }
}