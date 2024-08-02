namespace Trailblazor.Routing.Extensions;

internal static class DictionaryExtensions
{
    internal static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other)
        where TKey : notnull
    {
        foreach (var keyValuePair in other)
            dictionary[keyValuePair.Key] = keyValuePair.Value;

        return dictionary;
    }
}
