using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Day17.Challenge2;

public partial class Interpreter
{
    private readonly bool _debug;
    public long RegisterA;
    public long RegisterB;
    public long RegisterC;
    public char[] Program;
    
    private int ProgramPointer = 0;
    
    public List<char> Output = [];
    
    
    public Interpreter(string input,bool debug = false)
    {
        _debug = debug;
        var parts = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        RegisterA = long.Parse(ParseRegister().Match(parts[0]).Groups["value"].Value);
        RegisterB = long.Parse(ParseRegister().Match(parts[1]).Groups["value"].Value);
        RegisterC = long.Parse(ParseRegister().Match(parts[2]).Groups["value"].Value);
        Program = ParseProgram().Match(parts[3]).Groups["program"].Value.ToCharArray().Where(char.IsDigit)
            .ToArray();
    }


    public void Restart(long registerA, long registerB, long registerC)
    {
        RegisterA = registerA;
        RegisterB = registerB;
        RegisterC = registerC;
        ProgramPointer = 0;
        Output.Clear();
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
                    if (_debug)
                    {
                        Console.WriteLine(
                            $"A =  $A({RegisterA}) / 2^${operand}({ComboOperand(operand)}) = {RegisterA / (long)Math.Pow(2, ComboOperand(operand))}");
                        Debug.Assert(operand == '0' || operand == '1' || operand == '2' || operand == '3');
                    }

                    RegisterA = RegisterA / (long)Math.Pow(2, ComboOperand(operand));
                    break;
                //The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the
                //instruction's literal operand, then stores the result in register B.
                case '1':
                    if (_debug)
                        Console.WriteLine($"B = $B({RegisterB}) XOR {operand} = {RegisterB ^ (operand - '0')}");
                    RegisterB = RegisterB ^ (operand - '0');
                    break;
                // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
                // (thereby keeping only its lowest 3 bits), then writes that value to the B register.
                case '2':
                    if (_debug)
                        Console.WriteLine($"B = ${operand}({ComboOperand(operand)}) % 8 = {ComboOperand(operand)%8}");
                    RegisterB = ComboOperand(operand) % 8;
                    break;
                // The jnz instruction (opcode 3) does nothing if the A register is 0. However,
                // if the A register is not zero, it jumps by setting the instruction pointer to the
                // value of its literal operand; if this instruction jumps, the instruction pointer is
                // not increased by 2 after this instruction.
                case '3':
                    if (RegisterA != 0)
                    {
                        if (_debug)
                            Console.WriteLine($"pc = {operand}");
                        ProgramPointer = operand - '0';
                        // return true;
                    }

                    break;
                // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and
                // register C, then stores the result in register B. (For legacy reasons, this
                // instruction reads an operand but ignores it.)
                case '4':
                    if (_debug)
                        Console.WriteLine($"B = $B({RegisterB}) XOR $C{RegisterC} = {RegisterB ^ RegisterC}");
                    RegisterB = RegisterB ^ RegisterC;
                    break;
                // The out instruction (opcode 5) calculates the value of its combo operand
                // modulo 8, then outputs that value. (If a program outputs multiple values,
                // they are separated by commas.)
                case '5':
                    if (_debug)
                        Console.WriteLine($"PRINT ${operand}({ComboOperand(operand)}) % 8 = {ComboOperand(operand)%8}");
                    Output.Add((char)('0' + (ComboOperand(operand) % 8)));
                    break;
                // The bdv instruction (opcode 6) works exactly like the adv instruction
                // except that the result is stored in the B register. (The numerator is
                // still read from the A register.)
                case '6':
                    if (_debug)
                        Console.WriteLine($"B = $A({RegisterA}) / 2^${ComboOperand(operand)}({ComboOperand(operand)}) = { RegisterA / (long)Math.Pow(2, ComboOperand(operand))}");
                    RegisterB = RegisterA / (long)Math.Pow(2, ComboOperand(operand));
                    break;
                // The cdv instruction (opcode 7) works exactly like the adv instruction
                // except that the result is stored in the C register. (The numerator is
                // still read from the A register.)
                case '7':
                    if (_debug)
                        Console.WriteLine($"C = $A({RegisterA}) / 2^${ComboOperand(operand)}({ComboOperand(operand)}) = {RegisterA / (long)Math.Pow(2, ComboOperand(operand))}");
                    RegisterC = RegisterA / (long)Math.Pow(2, ComboOperand(operand));
                    break;
            }

            return false;
    }

    
    // 200111010000000
    // 25013876250000
    // 3126734531250

    public bool IsQuine() => Program.SequenceEqual(Output);
    

    public long ComboOperand(char operand)
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