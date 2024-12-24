using Day21.Challenge2;

var input = File.ReadAllLines("input.txt");


var total = 0L;
foreach(var code in input)
{
    var bot = new RobotHandler(code);
    total += bot.CalculateCodeComplexity();
    // Console.WriteLine(code);
    // Console.WriteLine(bot.Reverse(bot.instructions));
    Console.WriteLine(bot);
    // Console.WriteLine();
}

Console.WriteLine(total);