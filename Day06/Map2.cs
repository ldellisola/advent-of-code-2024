using System.Numerics;
using System.Text;

namespace Day06.Map2;

public class Map2
{


    public char[][] Map { get; set; }
    public Guard Guard { get; set; }
    public Guard InitialGuard { get;  set; }

    public HashSet<Guard> VisitedPositions { get;  set; }

    public bool IsGuardInLoop { get;  set; } = false;
    
    public Map2(){}
    public Map2(string file)
    {
        Map = File.ReadLines(file).Select(t => t.ToCharArray()).ToArray();
        Guard = FindGuard();
        InitialGuard = Guard;
        VisitedPositions = [Guard];
    }

    public Guard FindGuard()
    {
        foreach(var (iRow, row) in Map.Index())
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


    private readonly Vector _right = new(0, 1);
    private readonly Vector _left = new (0, -1);
    private readonly Vector _up = new (-1, 0);
    private readonly Vector _down = new (1, 0);

    public void Reset()
    {
        foreach(var pos in AllPositions())
        {
            if (this[pos] == '#')
                continue;
            this[pos] = '.';
        }
        IsGuardInLoop = false;
        Guard = InitialGuard;
        VisitedPositions = [Guard];
        this[Guard.Position] = Guard.Direction switch
        {
            (0, 1) => '>',
            (1, 0) => 'v',
            (0, -1) => '<',
            (-1, 0) => '^',
            _ => throw new ArgumentOutOfRangeException()
        };
        

    }
    
    public bool Move()
    {
        var nextPosition = Guard.Position + Guard.Direction;
        if(!IsInTheMap(nextPosition))
            return false;

        if(HasPassedThisPosition(nextPosition, Guard.Direction))
        {
            IsGuardInLoop = true;
            return false;
        }

        if(this[nextPosition] is '#' or 'O')
        {
            VisitedPositions.Add(Guard);
            Guard = Guard with { Direction = Guard.Direction switch
            {
                (0,1) =>  _down,
                (1,0) =>  _left,
                (0,-1) =>  _up,
                (-1,0) =>  _right,
                _ => throw new ArgumentOutOfRangeException()
            }};
            return true;
        }

        
        VisitedPositions.Add(Guard);
        this[Guard.Position] = 'X';
        Guard = Guard with { Position = nextPosition};
        this[nextPosition] = Guard.Direction switch
        {
            (0, 1) => '>',
            (1, 0) => 'v',
            (0, -1) => '<',
            (-1, 0) => '^',
            _ => throw new ArgumentOutOfRangeException()
        };
        
        return true;
    }


    public bool HasPassedThisPosition(Vector pos, Vector direction)
    {
        return VisitedPositions.Contains(new(pos,direction));
    }

    public char this[Vector v]
    {
        get
        {
            var (row, col) = v;
            return Map[row][col];
        }
        set
        {
            var (row, col) = v;
            Map[row][col] = value;
        }
    }
    
    public bool IsInTheMap(Vector position)
    {
        return position.Row >= 0 && position.Row < Map.Length && position.Col >= 0 && position.Col < Map[position.Row].Length;
    }

    public override string ToString()
    {
        var bld = new StringBuilder();
        foreach(var row in Map)
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

    public IEnumerable<Vector> AllPositions()
    {
        foreach(var row in Map.Index())
        {
            foreach(var col in row.Item.Index())
            {
                yield return new(row.Index,col.Index);
            }
        }
    }
}

public record Vector(int Row, int Col) : IAdditionOperators<Vector,Vector,Vector>
{
    public static Vector operator +(Vector left, Vector right)
    {
        return new Vector(left.Row + right.Row, left.Col + right.Col);
    }
}

public record Guard(Vector Position, Vector Direction);