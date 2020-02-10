using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace TestFileGenerator.Interfaces
{
    public interface ICustomCommand
    {
        Task OnExecuteAsync(CommandLineApplication command, IConsole console);
    }
}