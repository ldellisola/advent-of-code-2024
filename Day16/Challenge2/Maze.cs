using System.Text;
using Common;

namespace Day16.Challenge2;

using VectorWithDirection = (Vector position, Vector direction);
using VectorItem = (Vector position, Vector direction, List<(Vector position, Vector direction)> directions);


public class Maze(char[][] map)
{

    private long[][] costMap;
    public Maze(char[][] map, int i) : this(map)
    {
        costMap = new long[ map.Length ][];
        for(int row = 0; row < costMap.Length; row++)
        {
            costMap[row] = new long[map[row].Length];
            for(int col = 0; col < costMap[row].Length; col++)
            {
                if (map[row][col] == '#')
                    costMap[row][col] = -1;
                else
                    costMap[row][col] = long.MaxValue;
            }
        }
    }
    public long CalculateRun()
    {
        FloodFill();
        File.WriteAllText("text.csv",ToCsv());
        return Backtrack().Count;
    }

    public HashSet<Vector> Backtrack()
    {
        var start = Find('S');
        var end = Find('E');
        var visited = new HashSet<Vector>();
        var queue = new Queue<Vector>([end]);

        while(queue.TryDequeue(out var position))
        {
            if (!visited.Add(position))
                continue;

            if(position == start)
                return visited;

            Vector[] neighbors =
            [
                position + Vector.Down,
                position + Vector.Up,
                position + Vector.Left,
                position + Vector.Right,
            ];

            var minCostNeighbors = neighbors.GroupBy(t => costMap[t.Row][t.Col]).Where(t=> t.Key >=0).MinBy(t => t.Key).ToArray();

            foreach(var neighbor in minCostNeighbors.Reverse())
            {
                queue.Enqueue(neighbor);
            }
        }
        
        return [];
    }

    public string ToCsv()
    {
        var bld = new StringBuilder();
        for(int row = 0; row < costMap.Length; row++)
        {
            bld.AppendLine(string.Join(',', costMap[row]));
        }

        return bld.ToString();
    }

    private void FloodFill()
    {
        HashSet<Vector> visited = [];
        var queue = new PriorityQueue<VectorWithDirection, long>();
        var start = Find('S');
        var end = Find('E');
        queue.Enqueue((start, Vector.Right),0);

        while(queue.TryDequeue(out var item , out var cost))
        {
            var (position, direction) = item;
            costMap[position.Row][position.Col] = Math.Min(cost, costMap[position.Row][position.Col]);

            if (!visited.Add(position))
                continue;

            
            if (position == end)
                return;


            var leftDirection = direction.RotateLeft();
            var rightDirection = direction.RotateRight();
            (Vector nextPosition, Vector nextDirection, long extraCost)[] neighbors =
            [
                (position + direction, direction, 1),
                (position + rightDirection, rightDirection, 1 + 1000),
                (position + leftDirection, leftDirection, 1 + 1000)
            ];

            foreach(var (nextPosition, nextDirection, extraCost) in neighbors)
            {
                if (!visited.Contains(nextPosition) && costMap[nextPosition.Row][nextPosition.Col] != -1)
                    queue.Enqueue((nextPosition,nextDirection), cost + extraCost);
            }
        }
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