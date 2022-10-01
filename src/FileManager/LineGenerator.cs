using System.Text;
using FileManager.Interfaces;

namespace FileManager;

internal class LineGenerator : ILineGenerator
{
    private const string Lorem = "lorem ipsum dolor sit amet consectetur adipiscing elit proin id neque non massa sollicitudin cursus nullam tristique tortor quis viverra hendrerit quam enim gravida nisi eget iaculis arcu felis in quam quisque nec diam consequat magna laoreet aliquam at vel nibh consectetur turpis ut venenatis dui aenean sapien nulla rutrum vitae ultrices et maximus posuere ipsum integer vitae laoreet erat cras vel aliquam ante ut ut dui non nisi faucibus pharetra in at nulla";
    private readonly int _maxWordsPerLine;
    private readonly int _minWordsPerLine;
    
    private readonly Random _random;
    private string[] _values;

    public LineGenerator(int minWordsPerLine, int maxWordsPerLine)
    {
        _minWordsPerLine = minWordsPerLine;
        _maxWordsPerLine = maxWordsPerLine;
        _random = new Random();
    }

    private void Init(int linesNumber)
    {
        var words = Lorem.Split(" ");
        var builder = new StringBuilder();

        var uniqueValuesNumber = linesNumber / 2;
        
        _values = new string[linesNumber / 2];
        
        while (uniqueValuesNumber > 0)
        {
            uniqueValuesNumber--;
            var wordsNumber = _random.Next(_minWordsPerLine, _maxWordsPerLine+ 1);
            
            for (var j = 0; j < wordsNumber; j++)
            {
                var index = _random.Next(words.Length);
                builder.Append(' ');
                builder.Append(words[index]);
            }

            _values[uniqueValuesNumber] = builder.ToString();
            builder.Clear();
        }
    }

    public IEnumerable<string> NextLine(int linesNumber)
    {
        Init(linesNumber);
            
        var uniqueValuesNumber = linesNumber / 2;

        while (linesNumber > 0)
        {
            var value = _values[_random.Next(uniqueValuesNumber)];
            var number = _random.Next(linesNumber).ToString();
            yield return $"{number}.{value}";
            linesNumber--;
        }
    }
}