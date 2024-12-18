using System.Text;
using Common;

namespace Day18.Challenge2;

public class Map
{
    private readonly int[][] _map;
    private readonly Vector _start = new(0, 0);
    private readonly Vector _end;
    public Map(int width, int height, string[] input, int maxBytes)
    {
        _map = new int[height][];
        _end = new(height-1, width-1);
        for(int row = 0; row < height; row++)
        {
            _map[row] = new int[width];
            for(int col = 0; col < width; col++)
            {
                _map[row][col] = int.MaxValue;
            }
        }

        foreach(var str in input.Take(maxBytes))
        {
            var parts = str.Split(",");
            var (col, row) = (int.Parse(parts[0]), int.Parse(parts[1]));
            _map[row][col] = -1;
        }
    }


    public int FindLeastAmountOfSteps()
    {
        FloodFill();
        return _map[_end.Row][_end.Col];
    }


    private void FloodFill()
    {
        HashSet<Vector> visited = [];
        var queue = new PriorityQueue<Vector,int>();
        queue.Enqueue(_start,0);
        
        while(queue.TryDequeue(out var position, out var cost))
        {
            if (!visited.Add(position))
                continue;
            _map[position.Row][position.Col] = cost;
            
            if(position == _end)
            {
                return;
            }

            Vector[] neighbors =
            [
                position + Vector.Down,
                position + Vector.Up,
                position + Vector.Left,
                position + Vector.Right,
            ];

            foreach(var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && IsInMap(neighbor) && _map[neighbor.Row][neighbor.Col] != -1)
                    queue.Enqueue(neighbor, cost + 1);
            }
        }
    }

    private bool IsInMap(Vector position)
    {
        return position.Row >= 0 && position.Row < _map.Length && position.Col >= 0 && position.Col < _map[0].Length;
    }

    public override string ToString()
    {
        var bld = new StringBuilder();
        
        for(int row = 0; row < _map.Length; row++)
        {
            for(int col = 0; col < _map[0].Length; col++)
            {
                bld.Append(_map[row][col] switch
                {
                    -1 => '#',
                    _ => '.'
                });
            }
            bld.AppendLine();
        }
        return bld.ToString();
    }


    public static int FindBlocker(int width, int height, string[] input)
    {
        var bytes = 1024;
        while(true)
        {
            var map = new Map(width, height, input, bytes);
            var steps = map.FindLeastAmountOfSteps();

            if(steps != int.MaxValue && new Map(width, height, input, bytes + 1).FindLeastAmountOfSteps() == int.MaxValue)
            {
                Console.WriteLine(bytes);
                Console.WriteLine(input[bytes]);
                return bytes;
            }

            if(steps == int.MaxValue)
                bytes -= bytes / 2;
            else
                bytes += bytes / 2;
        }
    }
}