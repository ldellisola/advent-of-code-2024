

using System.Text.Json;
using Day06.Map2;

var map = new Map2("input.txt");

var mapState = JsonSerializer.Serialize(map);

int loops = 0;
var positions = map.AllPositions().Where(t => map[t] != '#' && t != map.Guard.Position).ToArray();
int total = positions.Length;


Lock @lock = new();

Console.WriteLine($"Found {positions.Length} positions");
Parallel.ForEach(positions, new ParallelOptions
                            {
                                MaxDegreeOfParallelism = 32,
                            }, Execute);

Console.WriteLine(loops);

void Execute(Vector blocker)
{
    lock(@lock)
    {
        total--;
        Console.WriteLine($"Remaining positions: {total}");
    }
    
    var mapCopy = JsonSerializer.Deserialize<Map2>(mapState)!;
    mapCopy[blocker] = 'O';
    while(mapCopy.Move());
    if (mapCopy.IsGuardInLoop)
        Interlocked.Increment(ref loops);
}




