using System.Numerics;

namespace Day10.Challenge2;

public class TopographicMap(char[][] map)
{
    private static Vector _left = new(0, -1);
    private static Vector _right = new(0, 1);
    private static Vector _up = new(-1, 0);
    private static Vector _down = new(1, 0);

    public IEnumerable<Vector> FindAllTrailStarts()
    {
        foreach(var (iRow, row) in map.Index())
        {
            foreach(var (iCol, col) in row.Index())
            {
                if(map[iRow][iCol] == '0')
                    yield return new(iRow, iCol);
            }
        }
    }

    public int FindTrailHeadScore()
    {
        var starts = FindAllTrailStarts().ToArray();
        List<List<Vector>> trails = [];
        foreach(var start in starts)
        {
            var queue = new Queue<List<Vector>>([[start]]);

            while(queue.TryDequeue(out var trail))
            {
                var pos = trail[^1];
                
                if(this[pos] == '9')
                    trails.Add(trail);
                
                if(CanMove(pos, _right) )
                    queue.Enqueue([..trail, pos + _right]);
                if(CanMove(pos, _left))
                    queue.Enqueue([..trail, pos + _left]);
                if(CanMove(pos, _up))
                    queue.Enqueue([..trail, pos + _up]);
                if(CanMove(pos, _down))
                    queue.Enqueue([..trail,pos + _down]);
            }
        }

        return trails.Count;
    }

    private bool CanMove(Vector current, Vector direction)
    {
        var next = current + direction;
        return next.Row >= 0 && next.Row <= map.Length - 1 &&
               next.Col >= 0 && next.Col <= map[0].Length - 1 &&
               this[next] - this[current] == 1;
    }

    public char this[Vector v]
    {
        get
        {
            var (row, col) = v;
            return map[row][col];
        }
        set
        {
            var (row, col) = v;
            map[row][col] = value;
        }
    }
}