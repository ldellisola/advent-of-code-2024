using Common;

namespace Day16.Challenge1;

using VectorWithDirection = (Vector position, Vector direction);

public class Maze(char[][] map)
{
    public long CalculateRun()
    {
        var start = Find('S');
        var direction = Vector.Right;
        var result = FindPath(start, direction);
        return result;
    }

    private long FindPath(Vector initialPosition, Vector initialDirection)
    {
        HashSet<VectorWithDirection> visited = [];
        var stack = new PriorityQueue<VectorWithDirection,long>();
        
        stack.Enqueue((initialPosition,initialDirection),0);
        
        while (stack.TryDequeue(out var run, out var priority))
        {
            visited.Add(run);
            var pos = run.position;
            if (this[pos] == 'E')
            {
                return priority;
            }
            
            // straight ahead
            VectorWithDirection straight = (pos + run.direction, run.direction);
            if (!visited.Contains(straight) && this[straight.position] != '#')
                stack.Enqueue(straight,priority + 1);
            
            var left =  (pos, run.direction.RotateLeft());
            if (!visited.Contains(left))
                stack.Enqueue(left,priority + 1000);
            
            var right =  (pos, run.direction.RotateRight());
            if (!visited.Contains(right))
                stack.Enqueue(right,priority + 1000);
            
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