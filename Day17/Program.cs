using Day17.Challenge2;

var input  = File.ReadAllText("input.txt");

var interpreter = new Interpreter(input,false);
var registerB = interpreter.RegisterB;
var registerC = interpreter.RegisterC;


var numbers = new Queue<long>([ ..Enumerable.Range(0, 9)]);

long registerA =   5L*8*8*8*8*8*8*8*8*8*8*8*8*8*8*8
                 + 6L*8*8*8*8*8*8*8*8*8*8*8*8*8*8
                 + 0L*8*8*8*8*8*8*8*8*8*8*8*8*8
                 + 0L*8*8*8*8*8*8*8*8*8*8*8*8
                 + 6L*8*8*8*8*8*8*8*8*8*8*8
                 + 4L*8*8*8*8*8*8*8*8*8*8
                 + 4L*8*8*8*8*8*8*8*8*8
                 + 6L*8*8*8*8*8*8*8*8
                 + 7L*8*8*8*8*8*8*8
                 + 4L*8*8*8*8*8*8
                 + 0L*8*8*8*8*8
                 + 2L*8*8*8*8
                 + 4L*8*8*8
                 + 8L*8*8
                 + 5L*8
                 + 2
    ;
interpreter.Restart(registerA, registerB, registerC);
while (!interpreter.ExecuteInstruction());

Console.WriteLine(registerA);
Console.WriteLine($"Is Quine: {interpreter.IsQuine()}");
Console.WriteLine($"Program: {string.Join(',', interpreter.Program.Reverse())}");
Console.WriteLine($"Output:  {string.Join(',', interpreter.Output.AsEnumerable().Reverse())}");



