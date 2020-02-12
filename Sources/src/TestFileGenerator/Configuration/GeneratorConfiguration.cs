using System.Collections.Generic;
using System.ComponentModel.Design;

namespace TestFileGenerator.Configuration
{
    // TODO: extend the configuration for application!
    
    /// <summary>
    /// Simple configuration for the application.
    /// </summary>
    public class GeneratorConfiguration
    {
        public string OutputFolder { get; set; }
        
        public int DefaultStudentsCount { get; set; }
        public int DefaultSubjectsCount { get; set; }
        
        public string[] RussianSubjects { get; set; }
        public string[] EnglishSubjects { get; set; }
        
        public Dictionary<string, string> Dictionaries { get; set; }
    }
}