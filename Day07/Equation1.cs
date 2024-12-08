namespace Day07.Challenge1;
public record Equation(long Result, int[] Values)
{
    public static Equation Parse(string equation)
    {
        var separator = equation.IndexOf(':');
        var result = long.Parse(equation[..separator]);
        var values = equation[(separator + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        return new Equation(result, values);
    }

    public bool IsValid()
    {
        return GenerateCalculations(Values).Any(t=> Calculate(t) == Result);;
    }

    private static long Calculate(string equation)
    {
        long value = 0;
        char operatorChar = '+';
        foreach (var part in equation.Split(" ", StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries))
        {
            if (part == "+")
                operatorChar = '+';
            else if (part == "*")
                operatorChar = '*';
            else
            {
                if (operatorChar == '+')
                {
                    value += int.Parse(part);
                }
                else
                {
                    value *= int.Parse(part);
                }
            }
        }
        return value;
    }

    private static IEnumerable<string> GenerateCalculations(int[] values)
    {
        if (values is [var single])
            return [$"{single}"];
        
        var first = values[0];
        var rest = values.Skip(1).ToArray();

        var list = new List<string>();
        foreach (var calculation in GenerateCalculations(rest))
        {
            list.Add($"{first} + {calculation}");
            list.Add($"{first} * {calculation}");
        }
        return list;
    }
}