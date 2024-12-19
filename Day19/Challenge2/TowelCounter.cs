namespace Day19.Challenge2;

public class TowelCounter
{
    private readonly string[] _towelPatterns;
    private readonly string[] _towels;
    private readonly Dictionary<string,long> _cache = [];
    private readonly Dictionary<string,long>.AlternateLookup<ReadOnlySpan<char>> _realCache;
    
    public TowelCounter(string input)
    {
        string[] parts = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries);
        _towelPatterns = parts[0].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
        _towels = parts[1..];
        _realCache = _cache.GetAlternateLookup<ReadOnlySpan<char>>();
    }

    public long CountPossibleTowels()
    {
        long count = 0;

        foreach(var towel in _towels)
        {
           var c = Matches(towel);
           Console.WriteLine($"{towel}: {c}");
           count += c;
        }

        return count;
    }

    public long Matches(ReadOnlySpan<char> towel)
    {
        if (_realCache.TryGetValue(towel, out var count))
            return count;
        
        count = 0;
        foreach(var pattern in _towelPatterns)
        {
            if(towel.StartsWith(pattern))
            {
                long c = 0;
                if(towel[pattern.Length..].Length == 0)
                    c = 1;
                else
                    c += Matches(towel[pattern.Length..]);
                count += c;
            }
        }

        _realCache[towel] = count;

        return count;
    }
}