using Day12.Challenge2;

var input = File.ReadLines("input.txt")
                .Select(t=> t.ToCharArray())
                .ToArray();
                
                
var garden = new Garden(input);

Console.WriteLine(garden.CalculateFencePrice());