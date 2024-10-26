namespace Trailblazor.Routing.Routes;

internal record RouteParentDescriptor
{
    internal RouteParentDescriptor(Type parentComponent, string? parentUri)
    {
        ParentComponent = parentComponent;
        ParentUri = parentUri;
    }

    internal Type ParentComponent { get; set; }
    internal string? ParentUri { get; set; }
}
