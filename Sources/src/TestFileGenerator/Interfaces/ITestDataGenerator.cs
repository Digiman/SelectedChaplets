using System.Threading.Tasks;
using TestFileGenerator.Enums;
using TestFileGenerator.Models;

namespace TestFileGenerator.Interfaces
{
    public interface ITestDataGenerator
    {
        Task<Group> GenerateDataAsync(int studentsCount, int subjectsCount, Language language);
    }
}