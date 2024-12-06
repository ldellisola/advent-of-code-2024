using System.Numerics;
using System.Text;

namespace Day06.Map1;
using Direction = (int, int);

public class Map1
{


    public char[][] map { get; }
    public Guard guard { get; private set; }

    public HashSet<Vector> VisitedPositions { get; }
    
    public Map1(string file)
    {
        map = File.ReadLines("input.txt").Select(t => t.ToCharArray()).ToArray();
        guard = FindGuard();
        VisitedPositions = [guard.Position];
    }

    public Guard FindGuard()
    {
        foreach(var (iRow, row) in map.Index())
        {
            foreach(var (iCol, cell) in row.Index())
            {
                switch(cell)
                {
                    case '^':
                        return new Guard(new(iRow, iCol), _up);
                    case 'v':
                        return new Guard(new(iRow, iCol), _down);
                    case '>':
                        return new Guard(new(iRow, iCol), _right);
                    case '<':
                        return new Guard(new(iRow, iCol), _left);
                }
            }
        }
        throw new Exception("No guard found");
    }


    private readonly Direction _right = (0, 1);
    private readonly Direction _left = (0, -1);
    private readonly Direction _up = (-1, 0);
    private readonly Direction _down = (1, 0);
    
    public bool Move()
    {
        var nextPosition = guard.Position + guard.Direction;
        if(!IsInTheMap(nextPosition))
            return false;

        if(this[nextPosition] == '#')
        {
            guard = guard with { Direction = guard.Direction switch
            {
                (0,1) =>  _down,
                (1,0) =>  _left,
                (0,-1) =>  _up,
                (-1,0) =>  _right,
                _ => throw new ArgumentOutOfRangeException()
            }};
            return true;
        }

        
        VisitedPositions.Add(nextPosition);
        this[guard.Position] = 'X';
        guard = guard with { Position = nextPosition};
        this[nextPosition] = guard.Direction switch
        {
            (0, 1) => '>',
            (1, 0) => 'v',
            (0, -1) => '<',
            (-1, 0) => '^',
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return true;
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
    
    public bool IsInTheMap(Vector position)
    {
        return position.Row >= 0 && position.Row < map.Length && position.Col >= 0 && position.Col < map[position.Row].Length;
    }

    public override string ToString()
    {
        var bld = new StringBuilder();
        foreach(var row in map)
        {
            foreach(var col in row)
            {
                bld.Append(col);
            }
            bld.AppendLine();
        }
        bld.AppendLine($"Unique Positions: {VisitedPositions.Count}");

        return bld.ToString();
    }
}

public record Vector(int Row, int Col) : IAdditionOperators<Vector,Direction,Vector>
{
    public static Vector operator +(Vector left, Direction right)
    {
        return new Vector(left.Row + right.Item1, left.Col + right.Item2);
    }
}

public record Guard(Vector Position, Direction Direction);