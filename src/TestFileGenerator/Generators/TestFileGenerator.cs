using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        private const int DefaultStudentsCount = 5;
        private const int DefaultSubjectsCount = 4;

        public TestFileGenerator(ILogger<TestFileGenerator> logger, ITestDataGenerator testDataGenerator,
            IFileSaver fileSaver)
        {
            _logger = logger;
            _testDataGenerator = testDataGenerator;
            _fileSaver = fileSaver;
        }

        public async Task GenerateCorrectFileAsync(int students, int subjects)
        {
            int studentsCount = 0;
            int subjectsCount = 0;

            studentsCount = students == 0 ? DefaultStudentsCount : students;
            subjectsCount = subjects == 0 ? DefaultSubjectsCount : subjects;

            // 1. Generate the data.
            var group = await _testDataGenerator.GenerateDataAsync(studentsCount, subjectsCount);

            // 2. Save to the file.
            var filename = GetCorrectFilename();

            _fileSaver.SaveDataToFile(filename, group);
        }

        public async Task GenerateWrongFileAsync(int students, int subjects)
        {
            int studentsCount = 0;
            int subjectsCount = 0;

            studentsCount = students == 0 ? DefaultStudentsCount : students;
            subjectsCount = subjects == 0 ? DefaultSubjectsCount : subjects;

            // 1. Generate the data.
            var group = await _testDataGenerator.GenerateDataAsync(studentsCount, subjectsCount);
            
            // 2. Modify some data to have wrong file as result.
            ModifyGroup(group);

            // 3. Save to the file.
            var filename = GetWrongFilename();

            _fileSaver.SaveDataToFile(filename, group);
        }

        private void ModifyGroup(Group group)
        {
            // remove 1 subject from the header
            var tmp = group.Subjects;
            group.Subjects  = new List<string>();
            for (var i = 0; i < tmp.Count - 1; i++)
            {
                group.Subjects.Add(tmp[i]);
            }
        }

        private string GetCorrectFilename()
        {
            return $"GeneratedFile_Correct.csv";
        }
        
        private string GetWrongFilename()
        {
            return $"GeneratedFile_Wrong.csv";
        }
    }
}