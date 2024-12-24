using Day22.Challenge2;
using number = ulong;

number[] input =
    // [1ul];
    File.ReadAllLines("input.txt")
        .Select(number.Parse)
        .ToArray();

int iterations = 2000;

var result = input
             .AsParallel()
             .SelectMany(t => Monkey.CalculateSecret(t, iterations))
             .GroupBy(t => t.sequence)
             .ToDictionary(t => t.Key, t => t.Sum(r => r.bestPrice));
             // .Aggregate<number, number>(0, (agg, item) => agg + item);

Console.WriteLine(result.MaxBy(t=> t.Value));

return;