namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Service provides configured <see cref="RoutingOptions"/>.
/// </summary>
internal sealed class RoutingOptionsProvider(RoutingOptions routingOptions) : IRoutingOptionsProvider
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
