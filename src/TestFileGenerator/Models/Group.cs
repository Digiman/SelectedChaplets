using System.Collections.Generic;

namespace TestFileGenerator.Models
{
    /// <summary>
    /// Group with students.
    /// </summary>
    public sealed class Group
    {
        /// <summary>
        /// List of students.
        /// </summary>
        public List<Student> Students { get; set; }

        /// <summary>
        /// List of subjects.
        /// </summary>
        public List<string> Subjects { get; set; }

        public Group()
        {
            Students = new List<Student>();
            Subjects = new List<string>();
        }
    }
}