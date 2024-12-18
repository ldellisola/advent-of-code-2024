using System.Text.RegularExpressions;

namespace Day17.Challenge1;

public partial class Interpreter
{
    private int RegisterA;
    private int RegisterB;
    private int RegisterC;
    private char[] Program;
    
    private int ProgramPointer = 0;
    
    public List<char> Output = [];
    
    
    public Interpreter(string input)
    {
        var parts = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        RegisterA = int.Parse(ParseRegister().Match(parts[0]).Groups["value"].Value);
        RegisterB = int.Parse(ParseRegister().Match(parts[1]).Groups["value"].Value);
        RegisterC = int.Parse(ParseRegister().Match(parts[2]).Groups["value"].Value);
        Program = ParseProgram().Match(parts[3]).Groups["program"].Value.ToCharArray().Where(t => char.IsDigit(t))
            .ToArray();
    }

    public bool ExecuteInstruction()
    {
        if (!IsProgramPointerInBounds())
            return true;

        
        var opCode = Program[ProgramPointer];
        var operand = Program[ProgramPointer + 1];
        ProgramPointer += 2;

        switch (opCode)
        {
            case '0':
                RegisterA = RegisterA / (int) Math.Pow(2,ComboOperand(operand));
                break;
            //The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the
            //instruction's literal operand, then stores the result in register B.
            case '1':
                RegisterB = RegisterB ^ (operand - '0');
                break;
            // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
            // (thereby keeping only its lowest 3 bits), then writes that value to the B register.
            case '2':
                RegisterB = ComboOperand(operand) % 8;
                break;
            // The jnz instruction (opcode 3) does nothing if the A register is 0. However,
            // if the A register is not zero, it jumps by setting the instruction pointer to the
            // value of its literal operand; if this instruction jumps, the instruction pointer is
            // not increased by 2 after this instruction.
            case '3':
                if (RegisterA != 0)
                    ProgramPointer = operand - '0';
                break;
            // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and
            // register C, then stores the result in register B. (For legacy reasons, this
            // instruction reads an operand but ignores it.)
            case '4':
                RegisterB = RegisterB ^ RegisterC;
                break;
            // The out instruction (opcode 5) calculates the value of its combo operand
            // modulo 8, then outputs that value. (If a program outputs multiple values,
            // they are separated by commas.)
            case '5':
                Output.Add( (char) ('0'+ (ComboOperand(operand) % 8)));
                break;
            // The bdv instruction (opcode 6) works exactly like the adv instruction
            // except that the result is stored in the B register. (The numerator is
            // still read from the A register.)
            case '6':
                RegisterB = RegisterA / (int) Math.Pow(2,ComboOperand(operand));
                break;
            // The cdv instruction (opcode 7) works exactly like the adv instruction
            // except that the result is stored in the C register. (The numerator is
            // still read from the A register.)
            case '7':
                RegisterC = RegisterA / (int) Math.Pow(2,ComboOperand(operand));
                break;
        }

        return false;
    }

    public int ComboOperand(char operand)
    {
        return operand switch
        {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => RegisterA,
            '5' => RegisterB,
            '6' => RegisterC,
            _ => throw new Exception($"Invalid operand: {operand}")
        };
    }
    
    
    private bool IsProgramPointerInBounds() => ProgramPointer >= 0 && ProgramPointer < Program.Length;


    [GeneratedRegex(@"Register (A|B|C): (?<value>\d+)")]
    private partial Regex ParseRegister();
    
    [GeneratedRegex(@"Program: (?<program>\d(,\d)*)")]
    private partial Regex ParseProgram();
}