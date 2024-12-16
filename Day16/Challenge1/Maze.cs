using Common;

namespace Day16.Challenge1;

using QueueItem = (long cost,Vector direction, List<Vector> path);

public class Maze(char[][] map)
{
    public long CalculateRun()
    {
        var start = FindStart();
        var direction = Vector.Right;
        var result = FindPath(start, direction);

        var path = result.path.ToHashSet();
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (path.Contains(new Vector(row, col)))
                    Console.Write('*');
                else if (map[row][col] is not '.' and not 'x')
                    Console.Write(map[row][col]);
                else
                    Console.Write(' ');
            }
            Console.WriteLine();
        }

        return result.cost;
    }

    private (long cost, List<Vector> path) FindPath(Vector initialPosition, Vector initialDirection)
    {
        List<(long cost, List<Vector> path)> finishedRunes = [];
        var stack = new PriorityQueue<QueueItem,long>();
        
        stack.Enqueue((0,initialDirection, [initialPosition]),0);
        
        while (stack.TryDequeue(out var run, out var priority))
        {
            var pos = run.path[^1];
            if (this[pos] == 'E')
            {
                return (run.cost,run.path);
                finishedRunes.Add((run.cost, run.path));
                continue;
            }
            
            if (this[pos] == '#')
                continue;
            
            // straight ahead
            var straight = pos + run.direction;
            if (!run.path.Contains(straight))
                stack.Enqueue((run.cost + 1, run.direction, run.path.Concat([straight]).ToList()),run.cost + 1);
            
            var left =  run.direction.RotateLeft() + pos ;
            if (!run.path.Contains(left))
                stack.Enqueue((run.cost + 1000 + 1, run.direction.RotateLeft(),  run.path.Concat([left]).ToList()),run.cost + 1000 + 1);
            
            var right =  run.direction.RotateRight() + pos;
            if (!run.path.Contains(right))
                stack.Enqueue((run.cost + 1000 + 1, run.direction.RotateRight(),  run.path.Concat([right]).ToList()),run.cost + 1000 + 1);
            
            
        }
        
        return finishedRunes.MinBy(t=>t.cost);
    }


    // private (long cost, Stack<Vector> path) FindPath12345(Vector position, Vector direction, HashSet<Vector> visited)
    // {
    //     if (this[position] == 'E')
    //         return (0,new Stack<Vector>([position]));
    //     
    //     
    //     (long cost, Stack<Vector> path) straightScore = (long.MaxValue, []);
    //     if (this[position + direction] is '.' or 'E')
    //     {
    //         straightScore = FindPath(position + direction, direction,);
    //         straightScore.cost += 1;
    //         straightScore.path.Push(position);
    //     }
    //         
    //     
    //     (long cost, Stack<Vector> path) turnLeftScore = (long.MaxValue, []);
    //     var leftDirection = direction.RotateLeft();
    //     if (this[position + leftDirection] is '.' or 'E')
    //     {
    //         turnLeftScore = FindPath(position + leftDirection, leftDirection);
    //         turnLeftScore.cost += 1000 + 1;
    //         turnLeftScore.path.Push(position);
    //     }
    //     
    //     (long cost, Stack<Vector> path) turnRightScore = (long.MaxValue, []);
    //     var rightDirection = direction.RotateRight();
    //     if (this[position + rightDirection] is '.' or 'E')
    //     {
    //         turnRightScore = FindPath(position + rightDirection, rightDirection);
    //         turnRightScore.cost += 1000 + 1;
    //         turnRightScore.path.Push(position);
    //     }
    //
    //     return new[] {straightScore, turnLeftScore, turnRightScore}.MinBy(t=> t.cost);
    // }

    private Vector FindStart()
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == 'S')
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