namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that some routes share the same URI. That means the same URI has been specified either in the '@page' directive,
/// the builder pattern of any routing profile or when creating and registering a route by hand.
/// </summary>
public sealed class UriRegisteredToMultipleRoutesException : Exception
{
    internal UriRegisteredToMultipleRoutesException(string uri, List<Type> duplicateComponents)
        : base($"URI '{uri}' is registered to multiple components: {string.Join(", ", duplicateComponents.Select(t => t.Name))}")
    {
    }
}
