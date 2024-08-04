namespace Trailblazor.Routing.Exceptions;

public sealed class AbstractComponentException : Exception
{
    public AbstractComponentException(Type componentType)
        : base($"Cannot use an abstract component or interface to represent a route: {componentType.FullName}.")
    {

    }
}
