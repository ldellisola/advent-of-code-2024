using Day14.Challenge2;

var input = File.ReadAllLines("input.txt");

int i = int.TryParse(args[0], out var num) ? num : 0;



var map = new Map(input,101,103);
map.Run(Math.Max(i - 1,0));
Console.WriteLine(map);

do
{
    i++;
    map.Run(1);
    if (map.HasTree())
    {
        Console.Clear();
        Console.WriteLine(map);
        Console.WriteLine(i);
        Console.ReadKey(true);
    }

    // Thread.Sleep(100);
    
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
        break;
} while (true);




Console.WriteLine($"Total: {i}");
