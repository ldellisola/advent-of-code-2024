using System.Numerics;
using System.Text;

namespace Day14.Challenge2;

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
        foreach (var robot in _robots)
        {
            robot.CalculatePosition(seconds,_bounds);
        }

        return 0;
    }

    public bool HasTree()
    {
        var dic = _robots.Select(t=> t.Position).ToHashSet();

        for (int row = 0; row < _bounds.Y; row++)
        {
            for (int col = 0; col < _bounds.X; col++)
            {
                HashSet<Vector> tree = [
                    new (col, row),
                    
                    new (col, row+1),
                    new (col-1, row+1),
                    new (col+1, row+1),
                    
                    new (col, row+2),
                    new (col-1, row+2),
                    new (col-2, row+2),
                    new (col+1, row+2),
                    new (col+2, row+2),
                ];
                if (tree.All(t => dic.Contains(t)))
                    return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        var bld = new StringBuilder();
        var dic = _robots.Select(t=> t.Position).ToHashSet();

        for (int row = 0; row < _bounds.Y; row++)
        {
            for (int col = 0; col < _bounds.X; col++)
            {
                if (dic.Contains(new Vector(col, row)))
                    bld.Append('#');
                else
                    bld.Append(' ');
            }
            bld.AppendLine();
        }
        
        return bld.ToString();
    }
}