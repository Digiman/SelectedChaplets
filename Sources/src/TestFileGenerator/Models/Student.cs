using System.Collections.Generic;

namespace TestFileGenerator.Models
{
    public sealed class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        
        public List<int> Marks { get; set; }

        public Student()
        {
            Marks = new List<int>();
        }
    }
}