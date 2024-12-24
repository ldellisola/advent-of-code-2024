namespace Day23;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> values, int threshold)
    {
        var remaining = values;

        foreach (T value in values)
        {
            yield return value.Yield();

            if (threshold < 2)
            {
                continue;
            }

            remaining = remaining.Skip(1);

            foreach (var combination in GetCombinations(remaining, threshold - 1))
            {
                yield return value.Yield().Concat(combination);
            }
        }
    }


    public static IEnumerable<T[]> GetCombinationsList<T>(this IEnumerable<T> values, int threshold)
    {
        return GetCombinations<T>(values, threshold)
            .Select(t=> t.ToArray())
            ;
    }
    
    

    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }
}