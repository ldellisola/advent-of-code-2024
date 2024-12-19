namespace Day19.Challenge1;

public class TowelCounter
{
    private string[] _towelPatterns;
    private string[] _towels;
    private HashSet<string> cache = [];
    private HashSet<string>.AlternateLookup<ReadOnlySpan<char>> realCache;
    
    public TowelCounter(string input)
    {
        string[] parts = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries);
        _towelPatterns = parts[0].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
        _towels = parts[1..];
        realCache = cache.GetAlternateLookup<ReadOnlySpan<char>>();
    }

    public long CountPossibleTowels()
    {
        long count = 0;

        foreach(var towel in _towels)
        {
            if(Matches(towel))
                count++;
        }

        return count;
    }

    public bool Matches(ReadOnlySpan<char> towel)
    {
        if (realCache.Contains(towel))
            return true;
        
        foreach(var pattern in _towelPatterns)
        {
            var matches = towel.StartsWith(pattern) && (towel[pattern.Length..].Length == 0 || Matches(towel[pattern.Length..]));
            if(matches)
            {
                realCache.Add(towel);
                return true;
            }
        }

        return false;
    }
}