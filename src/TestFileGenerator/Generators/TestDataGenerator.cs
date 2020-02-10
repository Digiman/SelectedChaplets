using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Models;

namespace TestFileGenerator.Generators
{
    /// <summary>
    /// Sample test data to use in the generated files.
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// Sample data for possible Subjects.
        /// </summary>
        public static readonly string[] Subjects = new[]
        {
            "Математика", "Физика", "История", "Химия",
            "Русский язык", "Иностранные языки", "Черчение", "Труды",
            "Физкультура", "Психология", "Социология", "Экономика"
        };
    }

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
    
    public sealed class TestDataGenerator : ITestDataGenerator
    {
        public Task<Group> GenerateDataAsync(int studentsCount, int subjectsCount)
        {
            var group = new Group();
            
            // 1. Generate subjects.
            group.Subjects = GenerateSubjects(subjectsCount);
            
            // 2. 
            for (var i = 0; i < studentsCount; i++)
            {
                group.Students.Add(GenerateStudent(group.Subjects.Count));
            }
            
            return Task.FromResult(group);
        }

        private Student GenerateStudent(int subjectsCount)
        {
            var student = new Student();
            
            student.Name = RandomGenerator.RandomString(5);
            student.Surname = RandomGenerator.RandomString(10, true);
            student.LastName = RandomGenerator.RandomString(15, true);
            
            var rand = new Random();
            for (int i = 0; i < subjectsCount; i++)
            {
                student.Marks.Add(rand.Next(1, 10));
            }
            
            return student;
        }

        private List<string> GenerateSubjects(int subjectsCount)
        {
            int count = subjectsCount > TestData.Subjects.Length ? TestData.Subjects.Length : subjectsCount;
            var result = new List<string>();
            for (var i = 0; i < count; i++)
            {
                result.Add(TestData.Subjects[i]);
            }
            return result;
        }
    }
}