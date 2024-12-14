using System.Numerics;
using System.Text;

namespace Day14.Challenge1;

public class Map
{
    private readonly Vector _bounds;
    private readonly Robot[] _robots;

    public Map(string[] input, int width, int height)
    {
        _bounds = new (width, height);
        _robots = input.Select(t=> new Robot(t)).ToArray();
    }

    public int Run(int seconds)
    {
        var positions = _robots
            .Select(r => r.CalculatePosition(seconds,_bounds))
            .Aggregate(new Vector4(0,0,0,0), (agg, p) =>
            {
                int middleX = _bounds.X / 2;
                int middleY = _bounds.Y / 2;
                if (p.X < middleX && p.Y < middleY)
                    return agg + new Vector4(1,0,0,0);
                if (p.X > middleX && p.Y < middleY)
                    return agg + new Vector4(0,1,0,0);
                if (p.X < middleX && p.Y > middleY)
                    return agg + new Vector4(0,0,1,0);
                if (p.X > middleX && p.Y > middleY)
                    return agg + new Vector4(0,0,0,1);
                return agg;
            });
        
        return (int) (positions.X * positions.Y * positions.Z * positions.W);
    }

    public override string ToString()
    {
        var bld = new StringBuilder();
        var dic = _robots
            .GroupBy(t=> t.Position)
            .ToDictionary(t=> t.Key,t => t.Count());

        for (int row = 0; row < _bounds.Y; row++)
        {
            for (int col = 0; col < _bounds.X; col++)
            {
                if (dic.TryGetValue(new Vector(col, row), out var count))
                    bld.Append('#');
                else
                    bld.Append(' ');
            }
            bld.AppendLine();
        }
        
        return bld.ToString();
    }
}