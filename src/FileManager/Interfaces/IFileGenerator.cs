namespace FileManager.Interfaces;

public interface IFileGenerator
{
    string Create(int linesNumber, CancellationToken cancellationToken);
}