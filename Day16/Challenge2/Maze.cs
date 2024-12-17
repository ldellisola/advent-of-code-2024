using Common;

namespace Day16.Challenge2;

using VectorWithDirection = (Vector position, Vector direction);
using VectorItem = (Vector position, Vector direction, List<(Vector position, Vector direction)> directions);

public class Maze(char[][] map)
{
    public long CalculateRun()
    {
        var start = Find('S');
        var direction = Vector.Right;
        
        var (bestCost, maxLength) = FindPathSingle(start, direction);
        
        var result = FindPath(start, direction, bestCost, maxLength);
        return result;
    }

    private long FindPath(Vector initialPosition, Vector initialDirection, long maxCost, long maxLength)
    {
        var runs = new List<List<Vector>>();
        var stack = new PriorityQueue<VectorItem,long>();
        
        stack.Enqueue((initialPosition,initialDirection,[(initialPosition,initialDirection)]),0);
        ulong i = 0;
        
        while (stack.TryDequeue(out var run, out var priority))
        {
            i++;
            if (i % 100_000 == 0)
                Console.WriteLine($"Items remaining: {stack.Count:N0}");
            
            var pos = run.position;
            if (this[pos] == 'E' )
            {
                if (run.directions.Count == maxLength && priority == maxCost) 
                    runs.Add(run.directions.Select(t=> t.position).ToList());
                continue;
            }
            
            // straight ahead
            var straight = (pos: pos + run.direction, run.direction);
            if (!run.directions.Contains(straight) && this[straight.pos] != '#' && priority < maxCost && run.directions.Count < maxLength)
                stack.Enqueue((pos + run.direction, run.direction,run.directions.Concat([(pos + run.direction, run.direction)]).ToList()),priority + 1);
            
            var left =  (pos, run.direction.RotateLeft());
            if (!run.directions.Contains(left) && priority + 1000 < maxCost)
                stack.Enqueue((pos, run.direction.RotateLeft(), run.directions),priority + 1000);
            
            var right =  (pos, run.direction.RotateRight());
            if (!run.directions.Contains(right) && priority + 1000 < maxCost)
                stack.Enqueue((pos, run.direction.RotateRight(),run.directions),priority + 1000);
            
        }

        return new HashSet<Vector>(runs.SelectMany(t=> t)).Count;
    }
    
    private (long cost, long length) FindPathSingle(Vector initialPosition, Vector initialDirection)
    {
        HashSet<VectorWithDirection> visited = [];
        var stack = new PriorityQueue<(VectorWithDirection, long length),long>();
        
        stack.Enqueue(((initialPosition,initialDirection),1),0);
        
        while (stack.TryDequeue(out var run, out var priority))
        {
            visited.Add(run.Item1);
            var pos = run.Item1.position;
            if (this[pos] == 'E')
            {
                return (priority,run.length);
            }
            
            // straight ahead
            VectorWithDirection straight = (pos + run.Item1.direction, run.Item1.direction);
            if (!visited.Contains(straight) && this[straight.position] != '#')
                stack.Enqueue((straight,run.length+1),priority + 1);
            
            var left =  (pos, run.Item1.direction.RotateLeft());
            if (!visited.Contains(left))
                stack.Enqueue((left,run.length),priority + 1000);
            
            var right =  (pos, run.Item1.direction.RotateRight());
            if (!visited.Contains(right))
                stack.Enqueue((right,run.length),priority + 1000);
            
        }
        
        throw new Exception("No path found");
    }

    private Vector Find(char item)
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == item)
                    return new(row, col);
            }
        }
        throw new Exception("invaid map");
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