using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Day13.Challenge2;

public partial class Arcades
{
    public Arcades(string[] input)
    {
        foreach (var lines in input.Chunk(4))
        {
            var aMatch = ButtonParser().Match(lines[0]);
            var bMatch = ButtonParser().Match(lines[1]);
            var prizesMatch = PrizerParser().Match(lines[2]);
            
            decimal[] a =
            [
                decimal.Parse(aMatch.Groups["x"].Value),
                decimal.Parse(aMatch.Groups["y"].Value),
            ];

            decimal[] b =
            [
                decimal.Parse(bMatch.Groups["x"].Value),
                decimal.Parse(bMatch.Groups["y"].Value),
            ];

            decimal[] prizes =
            [
                decimal.Parse(prizesMatch.Groups["x"].Value) + 10_000_000_000_000,
                decimal.Parse(prizesMatch.Groups["y"].Value) + 10_000_000_000_000,
            ];

            decimal pressedB = (prizes[1] * a[0] - prizes[0] * a[1]) / (b[1] * a[0] - b[0] * a[1]);
            decimal pressedA = (prizes[0] - pressedB * b[0]) / a[0];
            
            if (pressedA.Scale != 0  || pressedB.Scale != 0)
                continue;
            
            Tokens += (long)pressedA * 3 + (long)pressedB;
        }
    }


    public long Tokens { get; set; } = 0;

    [GeneratedRegex(@"Button (A|B): X\+(?<x>\d+), Y\+(?<y>\d+)")]
    private partial Regex ButtonParser();
    
    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)")]
    private partial Regex PrizerParser();
}