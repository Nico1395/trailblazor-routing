using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Validation;

/// <summary>
/// Service validates routes.
/// </summary>
internal interface IInternalRouteValidator
{
    /// <summary>
    /// Method validates specified <paramref name="routes"/> for integrity and functionality.
    /// </summary>
    /// <param name="routes">Routes to be validated.</param>
    internal void ValidateRoutes(List<Route> routes);
}
