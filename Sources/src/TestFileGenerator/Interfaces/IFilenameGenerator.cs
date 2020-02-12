namespace TestFileGenerator.Interfaces
{
    public interface IFilenameGenerator
    {
        string GenerateCorrectFilename();
        string GenerateWrongFilename();
    }
}