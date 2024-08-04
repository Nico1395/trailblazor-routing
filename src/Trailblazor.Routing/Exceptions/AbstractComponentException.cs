namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that an abstract component or interface was associated with a route.
/// </summary>
public sealed class AbstractComponentException : Exception
{
    internal AbstractComponentException(Type componentType)
        : base($"Cannot use an abstract component or interface to represent a route: {componentType.FullName}.")
    {
    }
}
