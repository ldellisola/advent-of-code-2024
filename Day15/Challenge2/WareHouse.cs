using System.Text;

namespace Day15.Challenge2;

public class WareHouse
{
    private static readonly StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
    private readonly char[][] _map;
    private readonly Queue<char> _actions;
    private Vector _robot;
    public WareHouse(string input)
    {
        var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
        _map = parts[0].Split(Environment.NewLine,SplitOptions).Select(row => row.ToCharArray().SelectMany(TransformMap).ToArray() ).ToArray();
        _actions = new (parts[1].Split(Environment.NewLine, SplitOptions).SelectMany(t=> t.ToCharArray()));
        _robot = FindRobot();
    }

    private static char[] TransformMap(char c)
    {
        return c switch
        {
            '#' => ['#', '#'],
            '.' => ['.', '.'],
            'O' => ['[', ']'],
            '@' => ['@', '.']
        };
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

        switch (this[nextPosition], action)
        {
            case ('.',_):
                this[nextPosition] = '@';
                this[position] = '.';
                return nextPosition;
            case ('#',_):
                return position;
            case ('[', 'v'):
            case ('[', '^'):
                if (MoveBoxesVertically(nextPosition, nextPosition + Vector.Right, direction))
                {
                    this[nextPosition] = '@';
                    this[position] = '.';
                    return nextPosition;
                }
                return position;
            case ('[', '<'):
            case ('[', '>'):
            case (']', '<'):
            case (']', '>'):
                if (MoveBoxesHorizontally(nextPosition, direction))
                {
                    this[nextPosition] = '@';
                    this[position] = '.';
                    return nextPosition;
                }
                return position;
            case (']','v'):
            case (']','^'):
                if (MoveBoxesVertically( nextPosition + Vector.Left,nextPosition,direction))
                {
                    this[nextPosition] = '@';
                    this[position] = '.';
                    return nextPosition;
                }
                return position;
        }
        return position;
    }

    private bool MoveBoxesVertically(Vector openBracket, Vector closeBracket, Vector direction)
    {
        var nextOpenBracket = openBracket + direction;
        var nextCloseBracket = closeBracket + direction;

        switch (this[nextOpenBracket],this[nextCloseBracket])
        {
            case ('#',_):
            case (_,'#'):
                return false;
            case ('.','.'):
                this[nextOpenBracket] = this[openBracket];
                this[nextCloseBracket] = this[closeBracket];
                this[openBracket] = '.';
                this[closeBracket] = '.';
                return true;
            case ('[',']'):
                if (MoveBoxesVertically(nextOpenBracket, nextCloseBracket, direction))
                {
                    this[nextOpenBracket] = this[openBracket];
                    this[nextCloseBracket] = this[closeBracket];
                    this[openBracket] = '.';
                    this[closeBracket] = '.';
                    return true;
                }
                return false;
            case (']','.'):
                if (MoveBoxesVertically(nextOpenBracket + Vector.Left, nextOpenBracket, direction))
                {
                    this[nextOpenBracket] = this[openBracket];
                    this[nextCloseBracket] = this[closeBracket];
                    this[openBracket] = '.';
                    this[closeBracket] = '.';
                    return true;
                }
                return false;
            case (']','['):
                bool canMoveLeft = CanMoveBoxesVertically(nextOpenBracket+Vector.Left, nextOpenBracket, direction);
                bool canMoveRight = CanMoveBoxesVertically(nextCloseBracket, nextCloseBracket + Vector.Right, direction);
                if (canMoveLeft && canMoveRight)
                {
                    MoveBoxesVertically(nextOpenBracket + Vector.Left, nextOpenBracket, direction);
                    MoveBoxesVertically(nextCloseBracket, nextCloseBracket + Vector.Right, direction);
                    this[nextOpenBracket] = this[openBracket];
                    this[nextCloseBracket] = this[closeBracket];
                    this[openBracket] = '.';
                    this[closeBracket] = '.';
                    return true;
                }
                return false;
            case ('.','['):
                if (MoveBoxesVertically(nextCloseBracket, nextCloseBracket + Vector.Right, direction))
                {
                    this[nextOpenBracket] = this[openBracket];
                    this[nextCloseBracket] = this[closeBracket];
                    this[openBracket] = '.';
                    this[closeBracket] = '.';
                    return true;
                }
                return false;
        }

        return false;
    }

    private bool CanMoveBoxesVertically(Vector openBracket, Vector closeBracket, Vector direction)
    {
        var nextOpenBracket = openBracket + direction;
        var nextCloseBracket = closeBracket + direction;

        switch (this[nextOpenBracket], this[nextCloseBracket])
        {
            case ('#', _):
            case (_, '#'):
                return false;
            case ('.', '.'):
                return true;
            case ('[', ']'):
                return CanMoveBoxesVertically(nextOpenBracket, nextCloseBracket, direction);
            case (']', '.'):
                return CanMoveBoxesVertically(nextOpenBracket + Vector.Left, nextOpenBracket, direction);
            case (']', '['):
                bool canMoveLeft = CanMoveBoxesVertically(nextOpenBracket + Vector.Left, nextOpenBracket, direction);
                bool canMoveRight = CanMoveBoxesVertically(nextCloseBracket, nextCloseBracket + Vector.Right, direction);
                return canMoveLeft && canMoveRight;
            case ('.', '['):
                return CanMoveBoxesVertically(nextCloseBracket, nextCloseBracket + Vector.Right, direction);
        }
        return false;
    }
    
    private bool MoveBoxesHorizontally(Vector position, Vector direction)
    {
        var nextPosition = position + direction;
        switch (this[nextPosition])
        {
            case '.':
                this[nextPosition] = this[position];
                this[position] = '.';
                return true;
            case '#':
                return false;
            case '[':
            case ']':
                if (MoveBoxesHorizontally(nextPosition, direction))
                {
                    this[nextPosition] = this[position];
                    this[position] = '.';
                    return true;
                }
                return false;
        }
        return false;
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
                if (_map[row][col] == '[')
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