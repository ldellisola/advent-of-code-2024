using System.Numerics;
using System.Text.RegularExpressions;

namespace Day14.Challenge2;

public partial class Robot
{
    public Vector Position { get; set; }
    public Vector Velocity { get; set; }
    public Robot(string input)
    {
        var match = Parser().Match(input);
        Position = new (int.Parse(match.Groups["col"].Value), int.Parse(match.Groups["row"].Value));
        Velocity = new (int.Parse(match.Groups["colSpeed"].Value), int.Parse(match.Groups["rowSpeed"].Value));
    }

    public Vector CalculatePosition(int seconds, Vector bounds)
    {
        return Position =  new (
            Reminder((seconds * Velocity.X) +  Position.X, bounds.X),
            Reminder((seconds * Velocity.Y) + Position.Y, bounds.Y)
        );
    }

    private static int Reminder(int value, int bound)
    {
        int reminder = (value % bound);
        if (reminder >= 0)
            return reminder;
        return  reminder + bound;
    }
    
    [GeneratedRegex(@"(?<place>p=(?<col>\d+),(?<row>\d+)) (?<speed>v=(?<colSpeed>-?\d+),(?<rowSpeed>-?\d+))")]
    internal partial Regex Parser();
}