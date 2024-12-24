using System.Text.RegularExpressions;

namespace Day24.Challenge1;

public partial class Circuit
{
    Dictionary<string, bool> variables = [];
    Queue<(Instruction Instruction, string variable)> instructions = [];

    
    public Circuit(string input)
    {
        var content = input.Split($"{Environment.NewLine}{Environment.NewLine}");

        var variableDeclarations = content[0];
        var instructionDeclarations = content[1];
        foreach(var dec in variableDeclarations.Split(Environment.NewLine))
        {
            var parts = dec.Split(":", StringSplitOptions.TrimEntries);
            variables[parts[0]] = parts[1] == "1";
        }

        foreach(var dec in instructionDeclarations.Split(Environment.NewLine))
        {
            var and = AndExpression().Match(dec);
            if(and.Success)
            {
                instructions.Enqueue((new AndInstruction(and.Groups["left"].Value, and.Groups["right"].Value), and.Groups["result"].Value));
                continue;
            }

            var or = OrExpression().Match(dec);
            if(or.Success)
            {
                instructions.Enqueue((new OrInstruction(or.Groups["left"].Value, or.Groups["right"].Value), or.Groups["result"].Value));
                continue;
            }

            var xor = XorExpression().Match(dec);
            if(xor.Success)
                instructions.Enqueue((new XorInstruction(xor.Groups["left"].Value, xor.Groups["right"].Value), xor.Groups["result"].Value));
        }
    }


    public ulong Execute()
    {
        while(instructions.TryDequeue(out var i))
        {
            var (instruction, variable) = i;
            if(!instruction.TryExecute(variables, out var result))
            {
                instructions.Enqueue(i);
                continue;
            }

            variables.Add(variable, result);
        }

        return PrintZValue();
    }
    
    private ulong PrintZValue()
    {
        var zVariables = variables.Where(t => t.Key.StartsWith('z'))
                                  .OrderByDescending(t => t.Key)
                                  .ToArray();

        ulong number = 0;
        foreach(var (key, value) in zVariables)
        {
            Console.Write(value ? 1 : 0);
            int position = int.Parse(key.Trim('z'));
            if (value)
                number += (ulong) Math.Pow(2, position);
        }
        Console.WriteLine();
        return number;
    }
    
    
    
    [ GeneratedRegex(@"(?<left>(\d|\w)+) AND (?<right>(\d|\w)+) -> (?<result>(\d|\w)+)") ]
    private static partial Regex AndExpression();

    [ GeneratedRegex(@"(?<left>(\d|\w)+) OR (?<right>(\d|\w)+) -> (?<result>(\d|\w)+)") ]
    private static partial Regex OrExpression();

    [ GeneratedRegex(@"(?<left>(\d|\w)+) XOR (?<right>(\d|\w)+) -> (?<result>(\d|\w)+)") ]
    private static partial Regex XorExpression();
}