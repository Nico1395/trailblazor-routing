using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

/// <summary>
/// Service manages the current router context internally.
/// </summary>
internal sealed class InternalRouterContextManager(
    NavigationManager _navigationManager,
    IRouteParser _routeParser,
    IRouteProvider _routeProvider) : IInternalRouterContextManager
{
    private RouterContext _internalRouterContext = RouterContext.Empty();

    /// <summary>
    /// Method returns the current router context.
    /// </summary>
    /// <returns>Current <see cref="RouterContext"/>.</returns>
    public RouterContext GetRouterContext()
    {
        return _internalRouterContext;
    }

    /// <summary>
    /// Method updates the current router context.
    /// </summary>
    /// <returns>Updated <see cref="RouterContext"/>.</returns>
    public RouterContext UpdateAndGetRouterContext()
    {
        var relativeUriWithParameters = _navigationManager.GetRelativeUri();
        var relativeUri = _routeParser.RemoveQueryParameters(relativeUriWithParameters);
        var route = _routeProvider.FindRoute(relativeUri);
        var uriQueryParameters = _routeParser.ExtractQueryParameters(relativeUriWithParameters);
        var componentQueryParameters = GetComponentQueryParameters(route?.Component, uriQueryParameters);

        return _internalRouterContext = RouterContext.New(
            relativeUriWithParameters,
            relativeUri,
            uriQueryParameters,
            componentQueryParameters,
            route);
    }

    private Dictionary<string, object?> GetComponentQueryParameters(Type? componentType, Dictionary<string, object?> uriQueryParameters)
    {
        if (componentType == null)
            return [];

        var componentQueryParameterProperties = componentType
            .GetProperties()
            .Where(p =>
                p.GetCustomAttribute<SupplyParameterFromQueryAttribute>() != null &&
                p.GetCustomAttribute<ParameterAttribute>() != null)
            .ToArray();

        return uriQueryParameters.Select(queryParameter =>
        {
            var queryParameterProperty = GetQueryParameterProperty(componentQueryParameterProperties, queryParameter);
            if (queryParameterProperty == null)
                return new KeyValuePair<string, object?>(string.Empty, default);

            return new KeyValuePair<string, object?>(queryParameterProperty.Name, queryParameter.Value);
        })
        .Where(p => p.Key != string.Empty)
        .ToDictionary();
    }

    private PropertyInfo? GetQueryParameterProperty(PropertyInfo[] componentQueryParameterProperties, KeyValuePair<string, object?> uriQueryParameter)
    {
        return componentQueryParameterProperties.SingleOrDefault(p =>
        {
            var queryParameterAttribute = p.GetCustomAttribute<SupplyParameterFromQueryAttribute>();

            // Use the attributes 'Name' property...
            if (queryParameterAttribute!.Name != null)
                return queryParameterAttribute.Name.Equals(uriQueryParameter.Key, StringComparison.CurrentCultureIgnoreCase);

            // ... otherwise use the properties name. Since that property has to be a parameter (we know that from the earlier filter), we can still use that property.
            return p.Name.Equals(uriQueryParameter.Key, StringComparison.CurrentCultureIgnoreCase);
        });
    }
}
