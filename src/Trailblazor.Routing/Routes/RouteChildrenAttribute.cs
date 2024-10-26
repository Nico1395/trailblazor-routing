namespace Trailblazor.Routing;

/// <summary>
/// Attribute configures the child components for a component.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RouteChildrenAttribute : Attribute
{
    /// <summary>
    /// Constructor configures the child components.
    /// </summary>
    /// <param name="childComponents">Child components for the component owning the attribute.</param>
    public RouteChildrenAttribute(params Type[] childComponents)
    {
        ChildrenComponents = childComponents;
    }

    /// <summary>
    /// Child components of the component.
    /// </summary>
    public Type[] ChildrenComponents { get; }
}
