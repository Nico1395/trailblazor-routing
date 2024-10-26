namespace Trailblazor.Routing.Routes;

/// <summary>
/// Attribute configures the parent component of a component.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RouteParentAttribute : Attribute
{
    /// <summary>
    /// Constructor configures the parent component of a component.
    /// </summary>
    /// <param name="parent">Parent component of the component owning the attribute.</param>
    public RouteParentAttribute(Type parent)
    {
        Parent = parent;
    }

    /// <summary>
    /// Parent component of the component.
    /// </summary>
    public Type Parent { get; }
}
