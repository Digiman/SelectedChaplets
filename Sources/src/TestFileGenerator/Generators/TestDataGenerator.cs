using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TestFileGenerator.Configuration;
using TestFileGenerator.Enums;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Models;

namespace TestFileGenerator.Generators
{
    // TODO: remove random generator from the logic later!

    public static class RandomGenerator
    {
        /// <summary>
        /// Generate a random string with a given size.
        /// </summary>
        /// <param name="size">Number of symbols.</param>
        /// <param name="lowerCase">Is use lower case?</param>
        /// <returns>Returns a generated string with random values.</returns>
        public static string RandomString(int size, bool lowerCase = false)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// Simple solution to generate test data to put in the file for test in the applications.
    /// </summary>
    public sealed class TestDataGenerator : ITestDataGenerator
    {
        private readonly GeneratorConfiguration _generatorConfiguration;

        public TestDataGenerator(IOptionsSnapshot<GeneratorConfiguration> options)
        {
            _generatorConfiguration = options.Value;
        }

        public Task<Group> GenerateDataAsync(int studentsCount, int subjectsCount, Language language)
        {
            var group = new Group();

            // 1. Generate subjects.
            group.Subjects = GenerateSubjects(subjectsCount, language);

            // 2. 
            for (var i = 0; i < studentsCount; i++)
            {
                group.Students.Add(GenerateStudent(group.Subjects.Count));
            }

            return Task.FromResult(group);
        }

        #region Helpers.
        
        private Student GenerateStudent(int subjectsCount)
        {
            var student = new Student();

            // generate name of the student with different algorithms
            var names = GenerateStudentNames();
            student.Name = names.name;
            student.Surname = names.surname;
            student.MiddleName = names.middleName;

            var rand = new Random();
            for (int i = 0; i < subjectsCount; i++)
            {
                student.Marks.Add(rand.Next(1, 10));
            }

            return student;
        }

        private (string name, string surname, string middleName) GenerateStudentNames()
        {
            // TODO: implement more functional logic here to generate the names!
            var syllabys = _generatorConfiguration.Dictionaries["Roman"];
            var generator = new NameGenerator(syllabys);
            var name = generator.Compose(2);
            var surname = generator.Compose(3);
            var middleName = generator.Compose(3);
            
            return (surname, name, middleName);
        }
        
        private (string, string, string) GenerateRandomStudentNames()
        { 
            var name = RandomGenerator.RandomString(5);
            var surname = RandomGenerator.RandomString(8, true);
            var middleName = RandomGenerator.RandomString(7, true);
            
            return (surname, name, middleName);
        }

        private List<string> GenerateSubjects(int subjectsCount, Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    return GenerateSubjects(_generatorConfiguration.RussianSubjects, subjectsCount);
                case Language.English:
                    return GenerateSubjects(_generatorConfiguration.EnglishSubjects, subjectsCount);
                default:
                    return new List<string>();
            }
        }

        private List<string> GenerateSubjects(string[] availableSubjects, in int subjectsCount)
        {
            int count = subjectsCount > availableSubjects.Length ? availableSubjects.Length : subjectsCount;
            var result = new List<string>();
            for (var i = 0; i < count; i++)
            {
                result.Add(availableSubjects[i]);
            }

            return result;
        }
        
        #endregion
    }
}