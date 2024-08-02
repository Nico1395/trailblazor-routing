using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Routes;

public sealed class RouteBuilder<TComponent>
    where TComponent : IComponent
{
    private readonly IRouteParser _routeParser;
    private readonly Route _route = Route.Empty<TComponent>();

    internal RouteBuilder(IRouteParser routeParser)
    {
        _routeParser = routeParser;
    }

    internal RouteBuilder(IRouteParser routeParser, Route parentRoute)
    {
        _routeParser = routeParser;
        _route.Parent = parentRoute;
    }

    public RouteBuilder<TComponent> WithMetadataValue(string key, object? value)
    {
        _route.SetMetadataValue(key, value);
        return this;
    }

    public RouteBuilder<TComponent> WithMetadata(Dictionary<string, object?> metadata)
    {
        _route.MergeMetadata(metadata);
        return this;
    }

    // TODO -> Parse this here or dynamically when fetching?
    public RouteBuilder<TComponent> WithUri(string uri)
    {
        _route.Uris = [_routeParser.ParseSegments(uri)];
        return this;
    }

    public RouteBuilder<TComponent> WithChild<TChildComponent>(Action<RouteBuilder<TChildComponent>> builderAction)
        where TChildComponent : IComponent
    {
        var builder = new RouteBuilder<TChildComponent>(_routeParser, _route);

        builderAction.Invoke(builder);
        _route.Children.Add(builder.Build());

        return this;
    }

    internal Route Build()
    {
        return _route;
    }
}
