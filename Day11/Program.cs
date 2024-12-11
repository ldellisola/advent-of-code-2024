using System.Diagnostics;
using Day11.Challenge2;

var input = File.ReadAllText("input.txt")
                .Split(" ",StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
                
var map = new Map(input, 75);

var timer = Stopwatch.StartNew();
map.Run();
timer.Stop();
Console.WriteLine($"Total time: {timer.Elapsed:g}");

Console.WriteLine(map.TotalRocks);