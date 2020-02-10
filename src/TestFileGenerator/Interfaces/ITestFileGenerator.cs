using System.Threading.Tasks;

namespace TestFileGenerator.Interfaces
{
    public interface ITestFileGenerator
    {
        Task GenerateCorrectFileAsync(int students, int subjects);
        Task GenerateWrongFileAsync(int students, int subjects);
    }
}