namespace Common;

public static class IEnumerableExtensions
{
    public static IEnumerable<(TSource first, TSource second)> ZipWithNext<TSource>(this IEnumerable<TSource> enumerable)
    {
        var a = enumerable.ToArray();
        var nextEnumerable = a.Skip(1);
        return a.Zip(nextEnumerable);
    }
}