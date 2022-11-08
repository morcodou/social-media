namespace System.Collections.Generic
{
    public static class EnumerableExtentions
    {
        public static bool SafeAny<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }
    }
}