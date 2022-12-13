namespace System.Collections.Generic;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        if (sequence.IsNullOrEmpty() || action == null)
            return;
        
        foreach (var item in sequence)
        {
            action(item);
        }
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();
}