using System.Threading.Tasks;
using TestFileGenerator.Models;

namespace TestFileGenerator.Interfaces
{
    public interface IFileSaver
    {
        void SaveDataToFile(string filename, Group group);
    }
}