namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Service provides configured <see cref="RoutingOptions"/>.
/// </summary>
public interface IRoutingOptionsAccessor
{
    /// <summary>
    /// Method returns the configured <see cref="RoutingOptions"/>.
    /// </summary>
    /// <returns>Configured <see cref="RoutingOptions"/></returns>
    public RoutingOptions GetRoutingOptions();
}
