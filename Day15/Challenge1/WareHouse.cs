using System.Text;

namespace Day15.Challenge1;

public class WareHouse
{
    private static readonly StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
    private readonly char[][] _map;
    private readonly Queue<char> _actions;
    private Vector _robot;
    public WareHouse(string input)
    {
        var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
        _map = parts[0].Split(Environment.NewLine,SplitOptions).Select(row => row.ToCharArray()).ToArray();
        _actions = new (parts[1].Split(Environment.NewLine, SplitOptions).SelectMany(t=> t.ToCharArray()));
        _robot = FindRobot();
    }

    public void RunRobotToEnd()
    {
        while (_actions.TryDequeue(out var action))
        {
            _robot = ExecuteAction(_robot,action);
        }
    }

    private Vector ExecuteAction(Vector position,char action)
    {
        var direction = action switch
        {
            '^' => Vector.Up,
            'v' => Vector.Down,
            '<' => Vector.Left,
            '>' => Vector.Right,
        };

        var nextPosition = position + direction;
        if (this[nextPosition] == '.')
        {
            this[nextPosition] = this[position];
            this[position] = '.';
            return nextPosition;
        }

        if (this[nextPosition] == '#')
        {
            return position;
        }
        
        if (this[nextPosition] == 'O')
        {
            var a = ExecuteAction(nextPosition, action);
            if (Equals(a, nextPosition))
                return position;
            var b = this[position];
            this[position] = '.';
            this[nextPosition] = b;
            return nextPosition;
        }

        return position;
    }

    private Vector FindRobot()
    {
        foreach(var (iRow, row) in _map.Index())
        {
            foreach(var (iCol, _) in row.Index())
            {
                if(_map[iRow][iCol] == '@')
                    return new (iRow, iCol);
            }
        }
        throw new Exception("No robot found");
    }
    
    public char this[Vector v]
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
    
    public long CalculateBoxesScore()
    {
        long total = 0;
        for (long row = 0; row < _map.Length; row++)
        {
            for (long col = 0; col < _map[row].Length; col++)
            {
                if (_map[row][col] == 'O')
                    total += 100 * row + col;
            }
        }

        return total;
    }
    
    public override string ToString()
    {
        var bld = new StringBuilder();

        for (int row = 0; row < _map.Length; row++)
        {
            for (int col = 0; col < _map[row].Length; col++)
            {
                bld.Append(_map[row][col]);
            }
            bld.AppendLine();
        }
        
        return bld.ToString();
    }
}