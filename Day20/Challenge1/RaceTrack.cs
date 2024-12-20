using System.Text;
using Common;

namespace Day20.Challenge1;

public class RaceTrack
{
    private readonly long[][] _map;
    private readonly Vector _start;
    private readonly Vector _end;
    
    public RaceTrack(char[][] input)
    {
        _map = new long[input.Length][];
        for(int row = 0; row < input.Length; row++)
        {
            _map[row] = new long[input[row].Length];
            for(int col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == '#')
                {
                    _map[row][col] = -1;
                    continue;
                }
                
                if (input[row][col] == 'S')
                    _start = new Vector(row, col);
                if (input[row][col] == 'E')
                    _end = new Vector(row, col);

                _map[row][col] = long.MaxValue;

            }
        }
        
        ArgumentNullException.ThrowIfNull(_start);
        ArgumentNullException.ThrowIfNull(_end);
    }

    public long CountCheats()
    {
        FloodFill();
        var bestPathWithoutCheating = GetBestPath();
        
        Vector[] directions =
        [
            Vector.Down,
            Vector.Up,
            Vector.Left,
            Vector.Right,
        ];

        long bestCost = this[_end];

        SortedDictionary<long, List<(Vector wall, Vector afterWall)>> cheats = [];

        foreach (var position in bestPathWithoutCheating)
        {
            foreach (var direction in directions)
            {
                var wall = position + direction;
                var afterWall = wall + direction;
                if (IsInMap(wall) && IsInMap(afterWall) && this[wall] == -1 && this[afterWall] != -1 && this[afterWall] != long.MaxValue)
                {
                    this[wall] = long.MaxValue;
                    FloodFill();
                    if (this[_end] < bestCost)
                    {
                        var savedTime = bestCost - this[_end] ;
                        if (cheats.TryGetValue(savedTime, out var list))
                            list.Add((wall, afterWall));
                        else
                            cheats[savedTime] = [(wall, afterWall)];
                    }
                    this[wall] = -1;
                    this[_end] = bestCost;
                }
            }
            
            
        }
        
        return cheats.Where(t=> t.Key >=100).Aggregate(0L, (a,b) => a += (b.Value.Count / 2));
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

    private List<Vector> GetBestPath()
    {
        HashSet<Vector> path = [];
        var queue = new Queue<Vector>();
        queue.Enqueue(_start);
        while (queue.TryDequeue(out var position))
        {
            if(!path.Add(position))
                continue;
            
            if (position == _end)
                return path.ToList();
            
            Vector[] neighbors =
            [
                position + Vector.Down,
                position + Vector.Up,
                position + Vector.Left,
                position + Vector.Right,
            ];
            
            long minCost = long.MaxValue;
            Vector? bestNeighbor = null;
            foreach(var neighbor in neighbors)
            {
                if (!path.Contains(neighbor) && IsInMap(neighbor) && _map[neighbor.Row][neighbor.Col] != -1 &&
                    _map[neighbor.Row][neighbor.Col] < minCost)
                {
                    minCost = _map[neighbor.Row][neighbor.Col];
                    bestNeighbor = neighbor;
                }
                   
            }
            if (bestNeighbor is not null)
                queue.Enqueue(bestNeighbor);
        }

        return [];
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
    
    public long this[Vector v]
    {
        get
        {
            var (row, col) = v;
            return _map[row][col];
        }
        set
        {
            var (row, col) = v;
            _map[row][col] = value;
        }
    }
}