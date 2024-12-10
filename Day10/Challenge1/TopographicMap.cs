using System.Numerics;

namespace Day10.Challenge1;

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

    public int FindTrailHeads()
    {
        var starts = FindAllTrailStarts().ToArray();
        int trailHeads = 0;
        foreach(var start in starts)
        {
            HashSet<Vector> visited = new();
            var queue = new Queue<Vector>([start]);

            while(queue.TryDequeue(out var position))
            {
                if(visited.Contains(position))
                    continue;
                if(this[position] == '9')
                    trailHeads++;
                visited.Add(position);

                if(CanMove(position, _right) && !visited.Contains(position + _right))
                    queue.Enqueue(position + _right);
                if(CanMove(position, _left) && !visited.Contains(position + _left))
                    queue.Enqueue(position + _left);
                if(CanMove(position, _up) && !visited.Contains(position + _up))
                    queue.Enqueue(position + _up);
                if(CanMove(position, _down) && !visited.Contains(position + _down))
                    queue.Enqueue(position + _down);
            }
        }

        return trailHeads;
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