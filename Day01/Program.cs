
var input = File.ReadLines("input.txt")
    .Select(t=> t.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray())
    .ToArray();
var list1 = input.Select(t => t[0]).Order().ToArray();
var list2 = input.Select(t => t[1]).Order().ToArray();

var distance  = list1.Zip(list2)
    .Select(t => Math.Abs(t.First - t.Second))
    .Sum();

Console.WriteLine("Challenge 1:");
Console.WriteLine(distance);


var weights = list2.CountBy(t => t).ToDictionary(t=> t.Key, t=> t.Value);
var newDistance = list1.Sum(t=> t * weights.GetValueOrDefault(t, 0));
Console.WriteLine($"Challenge 2:");
Console.WriteLine(newDistance);
 