using System.Text;
using Common;

namespace Day21.Challenge1;

public class RobotHandler(string input)
{
    private readonly int _codeValue = int.Parse(input.Replace("A", ""));
    private string robot1 = "";
    private string robot2 = "";
    public string instructions = "";

    public long CalculateCodeComplexity()
    {
        robot1 = ControlNumberPad(input);
        robot2 = ControlDirectionalPad(robot1);
        instructions = ControlDirectionalPad(robot2);

        return _codeValue * instructions.Length;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(input)
          .Append(':')
          .Append(' ')
          .Append(instructions.Length)
          .Append(" * ")
          .Append(_codeValue)
          .AppendLine()
            ;

        sb.AppendLine(instructions);
        sb.AppendLine(robot2);
        sb.AppendLine(robot1);
        return sb.ToString();
    }

    private Vector[] FromNumberPad(string input)
    {
        return input.Select(CharToNumpadPosition).ToArray();
    }

    private static Vector CharToNumpadPosition(char c)
    {
        return c switch
        {
            '7' => (0, 0),
            '8' => (0, 1),
            '9' => (0, 2),
            '4' => (1, 0),
            '5' => (1, 1),
            '6' => (1, 2),
            '1' => (2, 0),
            '2' => (2, 1),
            '3' => (2, 2),
            '0' => (3, 1),
            'A' => (3, 2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static char NumpadPositionToChar(Vector v)
    {
        return v switch
        {
            (0, 0) => '7',
            (0, 1) => '8',
            (0, 2) => '9',
            (1, 0) => '4',
            (1, 1) => '5',
            (1, 2) => '6',
            (2, 0) => '1',
            (2, 1) => '2',
            (2, 2) => '3',
            (3, 1) => '0',
            (3, 2) => 'A',
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Vector[] FromDirectionalPad(string input)
    {
        return input.Select(CharToDirectionPadPosition).ToArray();
    }

    private static Vector CharToDirectionPadPosition(char character)
    {
        return character switch
        {
            '^' => (0, 1),
            'A' => (0, 2),
            '<' => (1, 0),
            'v' => (1, 1),
            '>' => (1, 2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static char DirectionPadPositionToChar(Vector v)
    {
        return v switch
        {
            (0, 1) => '^',
            (0, 2) => 'A',
            (1, 0) => '<',
            (1, 1) => 'v',
            (1, 2) => '>',
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private string ControlDirectionalPad(string code)
    {
        var bld = new StringBuilder();
        Vector position = (0, 2);

        foreach(var nextPosition in FromDirectionalPad(code))
        {
            if(position == CharToDirectionPadPosition('^') && nextPosition == CharToDirectionPadPosition('<'))
            {
                bld.Append("v<");
                position = nextPosition;
            }
            else if(position == CharToDirectionPadPosition('A') && nextPosition == CharToDirectionPadPosition('<'))
            {
                bld.Append("v<<");
                position = nextPosition;
            }
            else if(position == CharToDirectionPadPosition('<') && nextPosition == CharToDirectionPadPosition('^'))
            {
                bld.Append(">^");
                position = nextPosition;
            }
            else if(position == CharToDirectionPadPosition('<') && nextPosition == CharToDirectionPadPosition('A'))
            {
                bld.Append(">>^");
                position = nextPosition;
            }
            else
            {
                while(position != nextPosition)
                {
                    var direction = position.GetDirection(nextPosition);
                    bld.Append(FromDirection(direction));
                    position += direction;
                    if(position == (0, 0))
                        throw new Exception("Fuck you for real");
                }
            }

            bld.Append('A');
        }

        return bld.ToString();
    }

    private string ControlNumberPad(string code)
    {
        var bld = new StringBuilder();
        Vector position = (3, 2);
        foreach(var nextPosition in FromNumberPad(code))
        {
            if(position == CharToNumpadPosition('A') && nextPosition == CharToNumpadPosition('1'))
            {
                bld.Append("^<<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('A') && nextPosition == CharToNumpadPosition('4'))
            {
                bld.Append("^^<<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('A') && nextPosition == CharToNumpadPosition('7'))
            {
                bld.Append("^^^<<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('0') && nextPosition == CharToNumpadPosition('1'))
            {
                bld.Append("^<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('0') && nextPosition == CharToNumpadPosition('4'))
            {
                bld.Append("^^<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('0') && nextPosition == CharToNumpadPosition('7'))
            {
                bld.Append("^^^<");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('1') && nextPosition == CharToNumpadPosition('0'))
            {
                bld.Append(">v");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('1') && nextPosition == CharToNumpadPosition('A'))
            {
                bld.Append(">>v");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('4') && nextPosition == CharToNumpadPosition('0'))
            {
                bld.Append(">vv");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('4') && nextPosition == CharToNumpadPosition('A'))
            {
                bld.Append(">>vv");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('7') && nextPosition == CharToNumpadPosition('0'))
            {
                bld.Append(">vvv");
                position = nextPosition;
            }
            else if(position == CharToNumpadPosition('7') && nextPosition == CharToNumpadPosition('A'))
            {
                bld.Append(">>vvv");
                position = nextPosition;
            }
            else
            {
                do
                {
                    var direction = position.GetDirection(nextPosition);
                    bld.Append(FromDirection(direction));
                    position += direction;

                    if(position == (3, 0))
                        throw new Exception("Fuck you");
                }
                while(position != nextPosition);
            }

            bld.Append('A');
        }

        return bld.ToString();
    }

    private static char FromDirection(Vector v)
    {
        return v switch
        {
            (0, 0) => 'A',
            (1, 0) => 'v',
            (-1, 0) => '^',
            (0, -1) => '<',
            (0, 1) => '>',
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
        };
    }

    private static Vector CharToDirectionVector(char c)
    {
        return c switch
        {
            'A' => (0, 0),
            'v' => (1, 0),
            '^' => (-1, 0),
            '<' => (0, -1),
            '>' => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    public string Reverse(string output)
    {
        var robot2Instructions = ReverseDirectionalPad((0, 2), output)
            .Aggregate("", (current, next) => current + next);

        var robot1Instructions = ReverseDirectionalPad((0, 2), robot2Instructions)
            .Aggregate("", (current, next) => current + next);

        var result = ReverseNumericPad((3, 2), robot1Instructions)
            .Aggregate("", (current, next) => current + next);

        return result;
    }

    private IEnumerable<char> ReverseNumericPad(Vector position, string code)
    {
        foreach(var section in code.Split('A').SkipLast(1))
        {
            position = section.Select(CharToDirectionVector).Aggregate(position, (current, next) => current + next);
            yield return NumpadPositionToChar(position);
        }
    }

    private IEnumerable<char> ReverseDirectionalPad(Vector position, string code)
    {
        foreach(var section in code.Split('A').SkipLast(1))
        {
            position = section.Select(CharToDirectionVector).Aggregate(position, (current, next) => current + next);
            yield return DirectionPadPositionToChar(position);
        }
    }
}