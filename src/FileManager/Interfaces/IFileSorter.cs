namespace FileManager.Interfaces;

public interface IFileSorter
{ 
    string Sort(string fileName, CancellationToken cancellationToken);
}