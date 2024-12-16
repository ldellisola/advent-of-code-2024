using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra.Single;

namespace Day13.Challenge1;

public partial class Arcades
{
    public Arcades(string[] input)
    {
        var tokenPrice = DenseMatrix.OfRowArrays([[3, 1]]);
        foreach (var lines in input.Chunk(4))
        {
            var a = ButtonParser().Match(lines[0]);
            var b = ButtonParser().Match(lines[1]);
            var prizes = PrizerParser().Match(lines[2]);
            var mat = DenseMatrix.OfColumnArrays([
                [int.Parse(a.Groups["x"].Value), int.Parse(a.Groups["y"].Value)],
                [int.Parse(b.Groups["x"].Value), int.Parse(b.Groups["y"].Value)]
            ]);
            var result = DenseMatrix.OfColumnArrays([
                [int.Parse(prizes.Groups["x"].Value), int.Parse(prizes.Groups["y"].Value)]
            ]);

            if (mat.Determinant() != 0)
            {
                var prod = mat.Inverse() * result;
                var price = tokenPrice * prod;

                if (!IsInteger(prod.At(0, 0)) || !IsInteger(prod.At(1, 0)))
                    continue;
                Tokens += (int) Math.Round(price.At(0, 0));
            }
        }
    }

    private bool IsInteger(float num)
    {
        return Math.Abs(num - Math.Round(num)) < 0.0001;
    }

    public int Tokens { get; set; } = 0;

    [GeneratedRegex(@"Button (A|B): X\+(?<x>\d+), Y\+(?<y>\d+)")]
    internal partial Regex ButtonParser();
    
    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)")]
    internal partial Regex PrizerParser();
}