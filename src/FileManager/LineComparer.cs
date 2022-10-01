namespace FileManager;

public class LineComparer : IComparer<string?>
{
    private const StringComparison ComparisonType = StringComparison.InvariantCulture;
    private const char Separator = '.';

    public int Compare(string? lineA, string? lineB)
    {
        lineA ??= string.Empty;
        lineB ??= string.Empty;
        
        var lineSpanA = lineA.AsSpan();
        var lineSpanB = lineB.AsSpan();
        var sepA = lineSpanA.IndexOf(Separator);
        var sepB = lineSpanB.IndexOf(Separator);

        if (sepA == -1 || sepB == -1)
        {
            return lineSpanA.CompareTo(lineSpanB, ComparisonType);
        }

        var textA = lineSpanA[(sepA + 1)..];
        var textB = lineSpanB[(sepB + 1)..];

        var compareResult = textA.CompareTo(textB, ComparisonType);

        if (compareResult == 0)
        {
            var numberA = int.Parse(lineSpanA[..sepA]);
            var numberB = int.Parse(lineSpanB[..sepB]);
            return numberA.CompareTo(numberB);
        }
        
        return compareResult;
    }
}