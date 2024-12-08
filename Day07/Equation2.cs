namespace Day07.Challenge2;
public record Equation(double Result, int[] Values)
{
    public static Equation Parse(string equation)
    {
        var separator = equation.IndexOf(':');
        var result = double.Parse(equation[..separator]);
        var values = equation[(separator + 1)..].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        return new Equation(result, values);
    }

    public bool IsValid()
    {
        return GenerateCalculations(Values).Any(t=> Math.Abs(Calculate(t) - Result) < 1);;
    }

    private static double Calculate(string equation)
    {
        double value = 0;
        char operatorChar = '+';
        foreach (var part in equation.Split(" ", StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries))
        {
            if (part == "+")
                operatorChar = '+';
            else if (part == "*")
                operatorChar = '*';
            else if (part == "||")
                operatorChar = '|';
            else
            {
                if (operatorChar == '+')
                {
                    value += int.Parse(part);
                }
                else if (operatorChar == '*')
                {
                    value *= int.Parse(part);
                }
                else
                {
                    value *= Math.Pow(10, part.Length);
                    value += int.Parse(part);
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
            list.Add($"{first} || {calculation}");
        }
        return list;
    }
}