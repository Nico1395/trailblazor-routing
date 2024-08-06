namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that multiple URIs have been found to be associated with the same component.
/// </summary>
public sealed class MultipleUrisFoundForComponentException : Exception
{
    internal MultipleUrisFoundForComponentException(Type componentType, int uriCount)
        : base($"{uriCount} URIs have been found for the component of type '{componentType}'. Cannot automatically differentiate what URI to navigate to.")
    {
    }
}
