namespace FileManager.Interfaces;

public interface ILineGenerator
{
    IEnumerable<string> NextLine(int linesNumber);
}