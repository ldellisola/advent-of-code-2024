namespace Day05;

public static class Parser
{
    public static IEnumerable<(int,int)> LoadOrderRules(StreamReader reader)
    {
        do
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                yield break;
            var span = line.AsSpan();
            var divider = span.IndexOf('|');
            
            yield return (int.Parse(span[..divider]), int.Parse(span[(divider + 1)..]));
        }while (!reader.EndOfStream);
    }

    public static Dictionary<int, HashSet<int>> ToDictionaryWithRepeated(this IEnumerable<(int, int)> enumerable)
    {
        var dictionary = new Dictionary<int, HashSet<int>>();

        foreach(var (first, second) in enumerable)
        {
            if (!dictionary.TryGetValue(second, out var value))
                dictionary.Add(second, [first]);
            else
                value.Add(first);
        }

        return dictionary;
    }

    public static IEnumerable<int[]> LoadSafetyManuals(StreamReader reader)
    {
        do
        {
            var line = reader.ReadLine();
            yield return line!.Split(',').Select(int.Parse).ToArray();
        }
        while (!reader.EndOfStream);
    }
}