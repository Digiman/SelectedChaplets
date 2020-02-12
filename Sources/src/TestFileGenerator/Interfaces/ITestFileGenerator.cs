using System.Threading.Tasks;
using TestFileGenerator.Enums;

namespace TestFileGenerator.Interfaces
{
    public interface ITestFileGenerator
    {
        Task GenerateCorrectFileAsync(int students, int subjects, Language language);
        Task GenerateWrongFileAsync(int students, int subjects, Language language);
    }
}