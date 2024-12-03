using System.Text.RegularExpressions;

var program = File.ReadAllText("input.txt");

Console.WriteLine("Challenge 1:");

var result = MultiplicationDetection().Matches(program)
                                         .Sum(t=> int.Parse(t.Groups["first"].Value) * int.Parse(t.Groups["second"].Value));

Console.WriteLine(result);

Console.WriteLine("--------------");
Console.WriteLine("Challenge 2:");

var results = SuperRegex().Matches(program).OrderBy(t => t.Index);

bool enabled = true;
var value = 0;
foreach(var match in results)
{
    if(match.Groups["disable"].Success)
    {
        enabled = false;
    }
    else if(match.Groups["enable"].Success)
    {
        enabled = true;
    }
    else if(match.Groups["mul"].Success)
    {
        if(enabled)
        {
            value += int.Parse(match.Groups["first"].Value) * int.Parse(match.Groups["second"].Value);
        }
    }
    
}
Console.WriteLine(value);



partial class Program
{
    [GeneratedRegex(@"mul\((?<first>\d+),(?<second>\d+)\)", RegexOptions.Compiled)]
    private static partial Regex MultiplicationDetection();
    
    [GeneratedRegex(@"(?<mul>mul\((?<first>\d+),(?<second>\d+)\))|(?<enable>do\(\))|(?<disable>don't\(\))")]
    private static partial Regex SuperRegex();
}
