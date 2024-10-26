namespace Trailblazor.Routing.Routes;

internal record RouteChildDescriptor
{
    internal RouteChildDescriptor(Type childComponent, string? childUri)
    {
        ChildComponent = childComponent;
        ChildUri = childUri;
    }

    internal Type ChildComponent { get; set; }
    internal string? ChildUri { get; set; }
}
