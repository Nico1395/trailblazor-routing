namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Service provides configured <see cref="RoutingOptions"/>.
/// </summary>
public interface IRoutingOptionsProvider
{
    /// <summary>
    /// Method returns the configured <see cref="RoutingOptions"/>.
    /// </summary>
    /// <returns>Configured <see cref="RoutingOptions"/></returns>
    public RoutingOptions GetRoutingOptions();
}
