namespace CloneDash;

internal static class IEnumerableExtensions
{
    public static T? RandomOrDefault<T>(this IEnumerable<T> source, Random? random = null)
        where T : class
    {
        T[] items = source.ToArray();
        return items.Length > 0 ? (random ?? new()).GetItems(items, 1)[0] : default;
    }
}
