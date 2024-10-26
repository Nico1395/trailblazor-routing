﻿using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing;

/// <summary>
/// Service provides configured <see cref="RoutingOptions"/>.
/// </summary>
internal sealed class RoutingOptionsAccessor(RoutingOptions routingOptions) : IRoutingOptionsAccessor
{
    private readonly RoutingOptions _routingOptions = routingOptions;

    /// <summary>
    /// Method returns the configured <see cref="RoutingOptions"/>.
    /// </summary>
    /// <returns>Configured <see cref="RoutingOptions"/></returns>
    public RoutingOptions GetRoutingOptions()
    {
        return _routingOptions;
    }
}
