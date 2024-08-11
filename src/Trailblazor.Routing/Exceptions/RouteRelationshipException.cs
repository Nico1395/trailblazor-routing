namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses the presence of an invalid relationship between routes.
/// </summary>
public sealed class RouteRelationshipException : Exception
{
    internal RouteRelationshipException(string message)
        : base(message)
    {
    }

    internal RouteRelationshipException(Type component, Type childComponent, Type childComponentParent)
        : base($"Route for component '{component.FullName}' cannot be configured with the route for component '{childComponent.FullName}' as a child, if that already has a route for component '{childComponentParent.FullName}' as a parent")
    {
    }
}
