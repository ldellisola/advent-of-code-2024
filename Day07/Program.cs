using Day07.Challenge2;

var equations = File.ReadAllLines("input.txt")
    .Select(Equation.Parse)
    .ToArray();

var result = equations.AsParallel()
    .Where(t => t.IsValid())
    .Sum(t => t.Result);

Console.WriteLine("Challenge 2");
Console.WriteLine(result);