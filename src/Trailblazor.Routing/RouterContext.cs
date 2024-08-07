using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Object represents the current context of the router.
/// </summary>
public sealed record RouterContext
{
    private RouterContext() { }

    /// <summary>
    /// Current relative URI with query parameters.
    /// </summary>
    public required string? RelativeUriWithParameters { get; init; }

    /// <summary>
    /// Current relative URI with query parameters.
    /// </summary>
    public required string? RelativeUri { get; init; }

    /// <summary>
    /// Query parameters of the current relative URI.
    /// </summary>
    /// <remarks>
    /// Note: If the route contains parameters setup via the '<c>@code</c>' directive, then this dictionary will be emtpy.
    /// </remarks>
    public required Dictionary<string, string> UriQueryParameters { get; init; }

    /// <summary>
    /// Query parameters originating from the query parameters from the current relative URI, that were able to be associated with
    /// the components parameters with a <see cref="SupplyParameterFromQueryAttribute"/> and matching query parameter name.
    /// </summary>
    /// <remarks>
    /// The key represents the properties name, while the value represents the parsed query parameter value.
    /// </remarks>
    public required Dictionary<string, object?> ComponentQueryParameters { get; init; }

    /// <summary>
    /// Route associated with the current relative URI.
    /// </summary>
    public required Route? Route { get; init; }

    /// <summary>
    /// Route data for the URI.
    /// </summary>
    public required RouteData? RouteData { get; init; }

    internal static RouterContext New(string relativeUriWithParameters, string relativeUri, Dictionary<string, string> uriQueryParameters, Dictionary<string, object?> componentQueryParameters, Route? route)
    {
        return new RouterContext()
        {
            RelativeUriWithParameters = relativeUriWithParameters,
            RelativeUri = relativeUri,
            UriQueryParameters = uriQueryParameters,
            ComponentQueryParameters = componentQueryParameters,
            Route = route,
            RouteData = route != null ? new RouteData(route.Component, componentQueryParameters) : null,
        };
    }

    internal static RouterContext Empty()
    {
        return new RouterContext()
        {
            RelativeUriWithParameters = string.Empty,
            RelativeUri = string.Empty,
            UriQueryParameters = [],
            ComponentQueryParameters = [],
            Route = null,
            RouteData = null,
        };
    }
}
