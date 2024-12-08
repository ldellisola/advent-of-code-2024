using Day08.Challange2;

var mapLayout = File.ReadLines("input.txt")
    .Select(t=> t.ToCharArray())
    .ToArray();

var frequencies = mapLayout.SelectMany(t=> t).Where(t=> t != '.').Distinct().ToArray();

var antiNodes = frequencies
    .AsParallel()
    .Select(frequency => new Map(mapLayout, frequency))
    .SelectMany(t => t.FindAntiNodes())
    .Distinct()
    .Count()
    ;

Console.Write("Challenge 2: ");
Console.WriteLine(antiNodes);


