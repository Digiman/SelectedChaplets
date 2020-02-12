using System.Globalization;
using System.IO;
using CsvHelper;
using TestFileGenerator.Enums;
using TestFileGenerator.Interfaces;
using TestFileGenerator.Models;

namespace TestFileGenerator.Savers
{
    /// <summary>
    /// Simple implementation of the logic to save file too CSV format.
    /// </summary>
    public sealed class CSVFileSaver : IFileSaver
    {
        public void SaveDataToFile(string filename, Group @group, Language language)
        {
            using (var file = File.Open(filename, FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        // 1. Write header.
                        WriteHeader(csvWriter, language);
                        foreach (var subject in group.Subjects)
                        {
                            csvWriter.WriteField(subject);
                        }
                        csvWriter.NextRecord();
                        
                        // 2. Write file data.
                        foreach (var student in group.Students)
                        {
                            csvWriter.WriteField(student.Name);
                            csvWriter.WriteField(student.Surname);
                            csvWriter.WriteField(student.MiddleName);

                            foreach (var mark in student.Marks)
                            {
                                csvWriter.WriteField(mark);
                            }
                            
                            csvWriter.NextRecord();
                        }
                    }
                }
            }
        }

        private void WriteHeader(CsvWriter csvWriter, Language language)
        {
            switch (language)
            {
                case Language.English:
                    csvWriter.WriteField("Surname");
                    csvWriter.WriteField("Name");
                    csvWriter.WriteField("Middle Name");
                    break;
                case Language.Russian:
                    csvWriter.WriteField("Фамилия");
                    csvWriter.WriteField("Имя");
                    csvWriter.WriteField("Отчество");
                    break;
            }
        }
    }
}